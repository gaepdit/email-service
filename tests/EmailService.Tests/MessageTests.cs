using GaEpd.EmailService;

namespace EmailService.Tests;

public class MessageTests
{
    [Test]
    public void Create_WithEmptySubject_Throws()
    {
        var func = () => Message.Create(subject: "", recipients: ["b"], senderEmail: "c", textBody: "d", htmlBody: null);
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Create_WithNoRecipients_Throws()
    {
        var func = () =>
            Message.Create(subject: "a", recipients: new List<string>(), senderEmail: "c", textBody: "d", htmlBody: null);
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Create_WithEmptyRecipient_Throws()
    {
        var func = () => Message.Create(subject: "a", recipients: [""], senderEmail: "c", textBody: "d", htmlBody: null);
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Create_WithNoBody_Throws()
    {
        var func = () => Message.Create(subject: "a", recipients: ["b"], senderEmail: "c", textBody: null, htmlBody: null);
        func.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Create_WithTextBody_DoesNotThrow()
    {
        var func = () => Message.Create(subject: "a", recipients: ["b"], senderEmail: "c", textBody: "d", htmlBody: null);
        func.Should().NotThrow();
    }

    [Test]
    public void Create_WithHtmlBody_DoesNotThrow()
    {
        var func = () => Message.Create(subject: "a", recipients: ["b"], senderEmail: "c", textBody: null, htmlBody: "d");
        func.Should().NotThrow();
    }
}
