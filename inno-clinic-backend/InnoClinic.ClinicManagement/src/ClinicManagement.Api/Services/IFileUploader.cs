namespace ClinicManagement.Api.Services;

public interface IFileUploader
{
    Task EnsureBucketExistsAsync(CancellationToken cancellationToken = default);

}
