using Azure.Storage.Blobs;
using ReenbitTestTask.Models;

namespace ReenbitTestTask.Services;

public class BlobStorageService(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("AzureBlobStorage")!;

    public async Task<string> UploadAsync(string containerName, BlobStorageForm form)
    {
        var container = new BlobContainerClient(_connectionString, containerName);
        await container.CreateIfNotExistsAsync();

        var blobName = SanitizeFileName(form.File!.Name);
        var blob = container.GetBlobClient(blobName);

        var response = await blob.UploadAsync(form.File.OpenReadStream(form.File.Size));
        return response.GetRawResponse().ToString();
    }

    private static string SanitizeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitizedFileName = string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');

        return sanitizedFileName;
    }
}