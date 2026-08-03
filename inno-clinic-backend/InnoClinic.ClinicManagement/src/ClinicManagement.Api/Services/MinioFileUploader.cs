using System;
using System.Threading.Tasks;

using Minio;
using Minio.Credentials;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace ClinicManagement.Api.Services;

public class MinioFileUploader(
    IMinioClient minioClient,
    ILogger<MinioFileUploader> logger) : IFileUploader
{
    private const string _bucketName = "innoclinic-files";

    public async Task EnsureBucketExistsAsync(CancellationToken cancellationToken = default)
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

    public async Task UploadFileAsync(string objectName, Stream data, string contentType)
    {
        try
        {
            var putArgs = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithStreamData(data)
                .WithObjectSize(data.Length)
                .WithContentType(contentType);

            await minioClient.PutObjectAsync(putArgs).ConfigureAwait(false);
        }
        catch (MinioException e)
        {
            logger.LogError(e, "File upload failed for object: {ObjectName}", objectName);
            throw;
        }
    }
}
