using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using ReenbitTestTask.Models;

namespace ReenbitTestTaskTests.TestTask.BlobStorageService;

public class UploadAsync
{
    [Fact]
    public async Task UploadsBlobWithMetadata()
    {
        // Arrange
        var connectionConfiguration = new Dictionary<string, string>
        {
            {"ConnectionStrings:AzureBlobStorage", "UseDevelopmentStorage=true"}
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(connectionConfiguration!)
            .Build();
        
        var containerName = "test-container";
        var email = "test@example.com";
        var fileSize = 512000;
        var stream = new MemoryStream(new byte[fileSize]);

        var mock = new Mock<IBrowserFile>();
        mock.SetupGet(x => x.Name).Returns("test-blob");
        mock.SetupGet(x => x.Size).Returns(fileSize);
        mock.Setup(x => x.OpenReadStream(fileSize, It.IsAny<CancellationToken>()))
            .Returns(stream);
        
        var form = new BlobStorageForm
        {
            File = mock.Object,
            Email = email
        };

        var service = new ReenbitTestTask.Services.BlobStorageService(configuration);

        // Act
        await service.UploadAsync(containerName, form);

        // Assert
        var containerClient = new BlobContainerClient(configuration.GetConnectionString("AzureBlobStorage"), containerName);
        var blobName = ReenbitTestTask.Services.BlobStorageService.SanitizeFileName(form.File.Name);
        var blobClient = containerClient.GetBlobClient(blobName);

        blobClient.ExistsAsync().Result.Value.Should().Be(true);

        var blobProperties = await blobClient.GetPropertiesAsync();
        blobProperties.Value.Metadata.Should().ContainKey("recipient");
        blobProperties.Value.Metadata["recipient"].Should().Be(form.Email);
    }
}