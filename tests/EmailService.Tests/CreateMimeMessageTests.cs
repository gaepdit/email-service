using AwesomeAssertions.Execution;
using GaEpd.EmailService;
using GaEpd.EmailService.Utilities;
using MimeKit;

namespace EmailService.Tests;

public class CreateMimeMessageTests
{
    private const string Email = "a@example.com";
    private const string Text = "a";

    [TestCase(Email)]
    [TestCase($"<{Email}>")]
    public void Create_WithValidEmail_Succeeds(string email)
    {
        // Arrange
        var message = Message.Create(subject: Text, recipients: [email], senderEmail: Email, textBody: Text,
            htmlBody: null);

        // Act
        var mimeMessage = EmailUtilities.CreateMimeMessage(message);

        // Assert
        using var scope = new AssertionScope();
        mimeMessage.Should().NotBeNull();
        mimeMessage.To[0].Should().NotBeNull();
        mimeMessage.To[0].Name.Should().BeEmpty();
        (mimeMessage.To[0] as MailboxAddress)!.Address.Should().Be(Email);
    }

    [TestCase("A", $"<{Email}>")]
    [TestCase("B, A", $"<{Email}>")]
    public void Create_WithValidEmailAndUser_Succeeds(string user, string email)
    {
        // Arrange
        var message = Message.Create(subject: Text, recipients: [$"{user} {email}"], senderEmail: Email, textBody: Text,
            htmlBody: null);

        // Act
        var mimeMessage = EmailUtilities.CreateMimeMessage(message);

        // Assert
        using var scope = new AssertionScope();
        mimeMessage.Should().NotBeNull();
        mimeMessage.To[0].Should().NotBeNull();
        mimeMessage.To[0].Name.Should().Be(user);
        (mimeMessage.To[0] as MailboxAddress)!.Address.Should().Be(Email);
    }
}
