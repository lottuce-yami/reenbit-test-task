using Azure.Communication.Email;
using Microsoft.Extensions.Configuration;

namespace ReenbitTestTaskTests.AzureFunction.EmailService;

public class SendMail
{
    
    [Fact]
    public void SendsMailOnce()
    {
        // Arrange
        var emailConfiguration = new Dictionary<string, string>
        {
            {"SENDER_ADDRESS", "DoNotReply@example.com"}
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(emailConfiguration!)
            .Build();

        var recipient = "test@example.com";
        var subject = "Test Subject";
        var message = "Test message";
        
        var mock = new Mock<EmailClient>();
        mock.Setup(x => x.SendAsync(
                It.IsAny<Azure.WaitUntil>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()
            )
        );
            
        var service = new ReenbitTestTaskAzureFunction.Services.EmailService(configuration, mock.Object);
        
        // Act
        service.SendMail(recipient, subject, message);
        
        // Assert
        mock.Verify(x => x.SendAsync(
                It.IsAny<Azure.WaitUntil>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()
            ), Times.Once
        );
    }
}