using Azure.Communication.Email;
using Microsoft.Extensions.Configuration;

namespace ReenbitTestTaskAzureFunction.Services;

public class EmailService
{
    private readonly IConfiguration _configuration;
    private readonly EmailClient _emailClient;

    public EmailService(IConfiguration configuration, EmailClient? emailClient = null)
    {
        _configuration = configuration;
        _emailClient = emailClient ?? new EmailClient(_configuration["COMMUNICATION_SERVICES_CONNECTION_STRING"]);
    }

    public void SendMail(string recipient, string subject, string message)
    {
        _emailClient.SendAsync(
            Azure.WaitUntil.Completed,
            senderAddress: _configuration["SENDER_ADDRESS"],
            recipientAddress: recipient,
            subject: subject,
            htmlContent: message
        );
    }
}