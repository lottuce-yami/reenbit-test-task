using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReenbitTestTaskAzureFunction.Services;

namespace ReenbitTestTaskAzureFunction
{
    public class BlobStorageTrigger
    {
        private readonly ILogger<BlobStorageTrigger> _logger;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public BlobStorageTrigger(ILogger<BlobStorageTrigger> logger, EmailService emailService, IConfiguration configuration)
        {
            _logger = logger;
            _emailService = emailService;
            _configuration = configuration;
        }

        [Function(nameof(BlobStorageTrigger))]
        public Task Run([BlobTrigger("documents/{name}")] BlobClient blobClient, string name)
        {
            var recipient = blobClient.GetPropertiesAsync().Result.Value.Metadata["recipient"]!;
            var uri = blobClient.Uri;
            
            _emailService.SendMail(
                recipient: recipient,
                subject: "Blob Storage Notification",
                message: $"Your file has been successfully uploaded. Link: {uri}"
            );
            
            return Task.CompletedTask;
        }
    }
}
