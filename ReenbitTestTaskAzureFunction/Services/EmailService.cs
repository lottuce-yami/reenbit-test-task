using Azure.Communication.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ReenbitTestTaskAzureFunction.Services;

public class EmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IConfiguration _configuration;
    
    public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public Task SendMail(string recipient, string subject, string message)
    {
        var emailClient = new EmailClient(_configuration["COMMUNICATION_SERVICES_CONNECTION_STRING"]);

        var emailSendOperation = emailClient.SendAsync(
            Azure.WaitUntil.Completed,
            senderAddress: _configuration["SENDER_ADDRESS"],
            recipientAddress: recipient,
            subject: subject,
            htmlContent: message
        );

        return Task.CompletedTask;
    }
}