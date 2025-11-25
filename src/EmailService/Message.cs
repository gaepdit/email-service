using System.Net.Mail;

namespace GaEpd.EmailService;

public record Message
{
    public string Subject { get; private set; } = string.Empty;
    public List<string> Recipients { get; } = [];
    public string? SenderName { get; private set; }
    public string? SenderEmail { get; private set; }
    public string? TextBody { get; private set; }
    public string? HtmlBody { get; private set; }
    public List<string> CopyRecipients { get; } = [];

    public static Message Create(string subject, string recipient, string? textBody, string? htmlBody,
        string? senderName = null, string? senderEmail = null, ICollection<string>? copyRecipients = null) =>
        Create(subject, [recipient], textBody, htmlBody, senderName, senderEmail, copyRecipients);

    public static Message Create(string subject, ICollection<string> recipients, string? textBody, string? htmlBody,
        string? senderName = null, string? senderEmail = null, ICollection<string>? copyRecipients = null)
    {
        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("A subject must be provided.", nameof(subject));

        if (recipients.Count == 0)
            throw new ArgumentException("At least one recipient must be specified.", nameof(recipients));

        if (recipients.Any(string.IsNullOrEmpty))
            throw new ArgumentException("Recipient cannot be null, empty, or white space.", nameof(recipients));

        if (!recipients.AreAllValid())
            throw new ArgumentException("Recipient list includes invalid emails.", nameof(recipients));

        if (!senderEmail.IsValid())
            throw new ArgumentException("Sender email is invalid.", nameof(senderEmail));

        if (copyRecipients != null && recipients.Count > 0 && !copyRecipients.AreAllValid())
            throw new ArgumentException("CC recipient list includes invalid emails.", nameof(copyRecipients));

        if (string.IsNullOrEmpty(htmlBody) && string.IsNullOrEmpty(textBody))
            throw new ArgumentException("Either a plaintext or HTML body must be provided.", nameof(htmlBody));

        var message = new Message
        {
            Subject = subject,
            SenderName = senderName,
            SenderEmail = senderEmail,
            TextBody = textBody,
            HtmlBody = htmlBody,
        };

        message.Recipients.AddRange(recipients);
        if (copyRecipients != null) message.CopyRecipients.AddRange(copyRecipients);

        return message;
    }
}

internal static class EmailExtensions
{
    public static bool AreAllValid(this ICollection<string> addresses) => addresses.All(IsValid);

    public static bool IsValid(this string? address)
    {
        if (string.IsNullOrEmpty(address)) return false;

        try
        {
            _ = new MailAddress(address);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
