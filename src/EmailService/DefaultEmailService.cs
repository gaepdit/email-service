using GaEpd.EmailService.Utilities;
using Microsoft.Extensions.Configuration;

namespace GaEpd.EmailService;

public class DefaultEmailService(IConfiguration configuration) : IEmailService
{
    private static EmailServiceSettings? Settings { get; set; }

    public async Task SendEmailAsync(Message message, CancellationToken token = default)
    {
        if (Settings is null)
        {
            Settings = new EmailServiceSettings();
            configuration.GetSection(nameof(EmailServiceSettings)).Bind(Settings);
        }

        // If emailing is disabled, do nothing.
        if (Settings is { EnableEmail: false, EnableEmailAuditing: false }) return;

        // Use the default sender name and email if none is provided.
        message.SenderName ??= Settings.DefaultSenderName;
        message.SenderEmail ??= Settings.DefaultSenderEmail;

        // If no sender email (or default) is provided, throw an exception.
        if (string.IsNullOrWhiteSpace(message.SenderEmail))
            throw new ArgumentException(
                "Either a Sender Email must be included with the Message or a Default Sender Email must be configured.");

        // Send requested email if enabled.
        if (Settings.EnableEmail)
        {
            var mimeMessage = EmailUtilities.CreateMimeMessage(message);
            await EmailUtilities.SendEmailAsync(mimeMessage, Settings!, token).ConfigureAwait(false);
        }

        // Send auditing email if enabled.
        if (Settings is not { EnableEmailAuditing: true, AuditEmailRecipients.Count: > 0 }) return;

        const string auditText = "This is a copy of the original email for auditing purposes.";
        var auditRecipients = $"Original recipient: {message.Recipients.ConcatWithSeparator(", ")}";

        message.TextBody = message.TextBody is null
            ? null
            : string.Concat(auditText, Environment.NewLine, auditRecipients, Environment.NewLine, "---",
                Environment.NewLine, Environment.NewLine, message.TextBody);

        message.HtmlBody = message.HtmlBody is null
            ? null
            : string.Concat("<em>", auditText, "<br>", auditRecipients, "</em><br><br>", message.HtmlBody);

        message.Recipients.Clear();
        message.Recipients.AddRange(Settings!.AuditEmailRecipients);
        message.CopyRecipients.Clear();

        var auditMimeMessage = EmailUtilities.CreateMimeMessage(message);
        await EmailUtilities.SendEmailAsync(auditMimeMessage, Settings, token).ConfigureAwait(false);
    }
}
