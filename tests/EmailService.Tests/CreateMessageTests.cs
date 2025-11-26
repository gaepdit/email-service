using GaEpd.EmailService;

namespace EmailService.Tests;

public class CreateMessageTests
{
    private const string ValidEmail = "a@example.com";

    [Test]
    public void Create_WithEmptySubject_Throws()
    {
        var func = () => Message.Create(subject: "", recipients: [ValidEmail], senderEmail: ValidEmail, textBody: "d",
            htmlBody: null);
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Create_WithNoRecipients_Throws()
    {
        var func = () =>
            Message.Create(subject: "a", recipients: new List<string>(), senderEmail: ValidEmail, textBody: "d",
                htmlBody: null);
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Create_WithEmptyRecipient_Throws()
    {
        var func = () =>
            Message.Create(subject: "a", recipients: [""], senderEmail: ValidEmail, textBody: "d", htmlBody: null);
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Create_WithInvalidRecipient_Throws()
    {
        var func = () => Message.Create(subject: "a", recipients: ["a.b.c"], senderEmail: ValidEmail, textBody: "d",
            htmlBody: null);
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Create_WithNoBody_Throws()
    {
        var func = () => Message.Create(subject: "a", recipients: [ValidEmail], senderEmail: ValidEmail, textBody: null,
            htmlBody: null);
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Create_WithTextBody_DoesNotThrow()
    {
        var func = () => Message.Create(subject: "a", recipients: [ValidEmail], senderEmail: ValidEmail, textBody: "d",
            htmlBody: null);
        func.Should().NotThrow();
    }

    [Test]
    public void Create_WithHtmlBody_DoesNotThrow()
    {
        var func = () => Message.Create(subject: "a", recipients: [ValidEmail], senderEmail: ValidEmail, textBody: null,
            htmlBody: "d");
        func.Should().NotThrow();
    }

    [Test]
    public void Create_WithTextAndHtmlBody_DoesNotThrow()
    {
        var func = () => Message.Create(subject: "a", recipients: [ValidEmail], senderEmail: ValidEmail, textBody: "d",
            htmlBody: "e");
        func.Should().NotThrow();
    }

    [Test]
    public void Create_WithInvalidCopyRecipient_Throws()
    {
        var func = () => Message.Create(subject: "a", recipients: [ValidEmail], senderEmail: ValidEmail, textBody: "d",
            htmlBody: null, copyRecipients: ["a.b.c"]);
        func.Should().Throw<ArgumentException>();
    }
}
