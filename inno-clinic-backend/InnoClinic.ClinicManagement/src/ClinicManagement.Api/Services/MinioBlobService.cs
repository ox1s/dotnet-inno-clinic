using System;
using System.Threading.Tasks;

using Minio;
using Minio.Credentials;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace ClinicManagement.Api.Services;

public class MinioBlobService(
    IMinioClient minioClient,
    ILogger<MinioBlobService> logger)
{
    private const string _bucketName = "innoclinic-files";

    public async Task GetOrCreateContainerAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var beArgs = new BucketExistsArgs()
                .WithBucket(_bucketName);

            bool found = await minioClient.BucketExistsAsync(beArgs, cancellationToken).ConfigureAwait(false);
            if (!found)
            {
                var mbArgs = new MakeBucketArgs()
                    .WithBucket(_bucketName);
                await minioClient.MakeBucketAsync(mbArgs, cancellationToken).ConfigureAwait(false);
                logger.LogInformation("Created Minio bucket: {BucketName}", _bucketName);
            }
        }
        catch (MinioException e)
        {
            logger.LogError(e, "Failed to create Minio bucket: {BucketName}", _bucketName);
            throw;
        }
    }

    public async Task<Guid> UploadAsync(Stream stream, string contentType,
        CancellationToken cancellationToken = default)
    {
        var fileId = Guid.NewGuid();

        try
        {
            var putArgs = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileId.ToString())
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType(contentType);

            await minioClient.PutObjectAsync(putArgs, cancellationToken).ConfigureAwait(false);
            
            return fileId;
        }
        catch (MinioException e)
        {
            logger.LogError(e, "File upload failed for object: {ObjectName}", fileId);
            throw;
        }
    }
}