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
        string? senderName = null, string? senderEmail = null, IEnumerable<string>? copyRecipients = null) =>
        Create(subject, [recipient], textBody, htmlBody, senderName, senderEmail, copyRecipients);

    public static Message Create(string subject, ICollection<string> recipients, string? textBody, string? htmlBody,
        string? senderName = null, string? senderEmail = null, IEnumerable<string>? copyRecipients = null)
    {
        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("A subject must be provided.", nameof(subject));

        if (recipients.Count == 0)
            throw new ArgumentException("At least one recipient must be specified.", nameof(recipients));

        if (recipients.Any(string.IsNullOrEmpty))
            throw new ArgumentException("Recipient cannot be null, empty, or white space.", nameof(recipients));

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
