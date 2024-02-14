using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ReenbitTestTaskAzureFunction
{
    public class BlobStorageTrigger
    {
        private readonly ILogger<BlobStorageTrigger> _logger;

        public BlobStorageTrigger(ILogger<BlobStorageTrigger> logger)
        {
            _logger = logger;
        }

        [Function(nameof(BlobStorageTrigger))]
        public Task Run([BlobTrigger("documents/{name}")] BlobClient blobClient, string name)
        {
            var uri = blobClient.Uri;
            _logger.LogInformation("C# Blob trigger function Processed blob\n Name: {name}\n URI: {uri}", name, uri);
            return Task.CompletedTask;
        }
    }
}
