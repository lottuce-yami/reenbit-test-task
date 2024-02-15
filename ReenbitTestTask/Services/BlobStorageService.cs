using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ReenbitTestTask.Models;

namespace ReenbitTestTask.Services;

public class BlobStorageService(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("AzureBlobStorage")!;

    public async Task UploadAsync(string containerName, BlobStorageForm form)
    {
        var container = new BlobContainerClient(_connectionString, containerName);
        await container.CreateIfNotExistsAsync();

        var blobName = SanitizeFileName(form.File!.Name);
        var metadata = new Dictionary<string, string> {{ "recipient", form.Email! }};
        var uploadOptions = new BlobUploadOptions { Metadata = metadata };
        var blob = container.GetBlobClient(blobName);
        
        await blob.UploadAsync(form.File.OpenReadStream(form.File.Size), uploadOptions);
    }

    private static string SanitizeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitizedFileName = string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');

        return sanitizedFileName;
    }
}