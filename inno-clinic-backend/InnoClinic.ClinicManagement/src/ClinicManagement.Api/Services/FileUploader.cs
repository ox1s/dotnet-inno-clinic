using System;
using System.Threading.Tasks;

using Minio;
using Minio.Credentials;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace ClinicManagement.Api.Services;

public class FileUploader(IMinioClient minioClient, ILogger<FileUploader> logger)
{
    private const string BucketName = "innoclinic-files";

    public async Task EnsureBucketExistsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var beArgs = new BucketExistsArgs()
                .WithBucket(BucketName);

            bool found = await minioClient.BucketExistsAsync(beArgs, cancellationToken).ConfigureAwait(false);
            if (!found)
            {
                var mbArgs = new MakeBucketArgs()
                    .WithBucket(BucketName);
                await minioClient.MakeBucketAsync(mbArgs, cancellationToken).ConfigureAwait(false);
                logger.LogInformation("Created Minio bucket: {BucketName}", BucketName);
            }
        }
        catch (MinioException e)
        {
            logger.LogError(e, "Failed to create Minio bucket: {BucketName}", BucketName);
            throw;
        }
    }

    public async Task UploadFileAsync(string objectName, Stream data, string contentType)
    {
        try
        {
            var putArgs = new PutObjectArgs()
                .WithBucket(BucketName)
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
