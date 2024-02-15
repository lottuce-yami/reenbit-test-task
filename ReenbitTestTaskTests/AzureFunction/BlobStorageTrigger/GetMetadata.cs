using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace ReenbitTestTaskTests.AzureFunction.BlobStorageTrigger;

public class GetMetadata
{
    [Fact]
    public void GetsMetadataCorrectly()
    {
        // Arrange
        var metadata = new Dictionary<string, string>
        {
            {"recipient", "test@example.com"}
        };
        var blobProperties = BlobsModelFactory.BlobProperties(metadata: metadata);
        
        var mock = new Mock<BlobClient>();
        var responseMock = new Mock<Response>();
        mock.Setup(x => x.GetPropertiesAsync(null, CancellationToken.None).Result)
            .Returns(Response.FromValue(blobProperties, responseMock.Object));
        
        // Act
        var result = ReenbitTestTaskAzureFunction.BlobStorageTrigger.GetMetadata(mock.Object, "recipient");

        // Assert
        result.Should().Be(metadata["recipient"]);
    }
}