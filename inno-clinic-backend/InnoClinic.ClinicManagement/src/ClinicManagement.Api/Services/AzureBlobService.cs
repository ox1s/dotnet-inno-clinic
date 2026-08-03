
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace ClinicManagement.Api.Services
{
    public class AzureBlobService(BlobServiceClient blobServiceClient) : IBlobService
    {
        private const string ContainerName = "files";

        private async Task<BlobContainerClient> GetOrCreateContainerAsync(CancellationToken cancellationToken = default)
        {
            var containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
            return containerClient;
        }

        public async Task DeleteAsync(Guid fileId, CancellationToken cancellationToken = default)
        {
            var containerClient = await GetOrCreateContainerAsync(cancellationToken);

            var blobClient = containerClient.GetBlobClient(fileId.ToString());

            await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        }

        public async Task<FileResponse> DownloadAsync(Guid fileId, CancellationToken cancellationToken = default)
        {
            var containerClient = await GetOrCreateContainerAsync(cancellationToken);

            var blobClient = containerClient.GetBlobClient(fileId.ToString());

            var response = await blobClient.DownloadContentAsync(cancellationToken: cancellationToken);

            return new FileResponse(response.Value.Content.ToStream(), response.Value.Details.ContentType);
        }

        public async Task<Guid> UploadAsync(Stream stream, string contentType, CancellationToken cancellationToken = default)
        {
            var containerClient = await GetOrCreateContainerAsync(cancellationToken);

            var fileId = Guid.NewGuid();
            var blobClient = containerClient.GetBlobClient(fileId.ToString());

            await blobClient.UploadAsync(
                stream,
                new BlobHttpHeaders { ContentType = contentType },
                cancellationToken: cancellationToken);

            return fileId;
        }
    }
}
