using GaEpd.EmailService.Utilities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace GaEpd.EmailService;

public class DefaultEmailService(IConfiguration configuration) : IEmailService
{
    private static EmailServiceSettings? Settings { get; set; }

    public Task SendEmailAsync(Message message, CancellationToken token = default)
    {
        if (Settings is null)
        {
            Settings = new EmailServiceSettings();
            configuration.GetSection(nameof(EmailServiceSettings)).Bind(Settings);
        }

        // If emailing is disabled, do nothing.
        if (Settings is { EnableEmail: false, EnableEmailAuditing: false })
            return Task.CompletedTask;

        if (message.SenderEmail is null && Settings.DefaultSenderEmail is null)
        {
            throw new ArgumentException(
                "Either a Sender Email must be included with the Message or a Default Sender Email must be configured.");
        }

        // Send requested email if enabled.
        if (Settings.EnableEmail && message.Recipients.Count > 0)
            SendRequestedEmail(message, token);

        // Send auditing email if enabled.
        if (Settings is { EnableEmailAuditing: true, AuditEmailRecipients.Count: > 0 })
            SendAuditingEmail(message, token);

        return Task.CompletedTask;
    }

    private static void SendRequestedEmail(Message message, CancellationToken token)
    {
        var mimeMessage = new MimeMessage();

        mimeMessage.From.Add(new MailboxAddress(message.SenderName ?? Settings!.DefaultSenderName,
            message.SenderEmail ?? Settings!.DefaultSenderEmail));
        mimeMessage.Subject = message.Subject;
        mimeMessage.To.AddRange(message.Recipients.Select(address => new MailboxAddress(string.Empty, address)));
        mimeMessage.Cc.AddRange(message.CopyRecipients.Select(address => new MailboxAddress(string.Empty, address)));

        var builder = new BodyBuilder
        {
            TextBody = message.TextBody,
            HtmlBody = message.HtmlBody,
        };
        mimeMessage.Body = builder.ToMessageBody();

        _ = EmailMessageAsync(mimeMessage, Settings!, token).ConfigureAwait(false);
    }

    private static void SendAuditingEmail(Message message, CancellationToken token)
    {
        var mimeMessage = new MimeMessage();

        mimeMessage.From.Add(new MailboxAddress(message.SenderName ?? Settings!.DefaultSenderName,
            message.SenderEmail ?? Settings!.DefaultSenderEmail));
        mimeMessage.Subject = message.Subject;
        mimeMessage.To.AddRange(Settings!.AuditEmailRecipients
            .Select(address => new MailboxAddress(string.Empty, address)));

        const string auditText = "This is a copy of the original email for auditing purposes. Original recipient: ";

        var auditBuilder = new BodyBuilder
        {
            TextBody = string.Concat(auditText, message.Recipients.ConcatWithSeparator(", "), Environment.NewLine,
                Environment.NewLine, message.TextBody),
            HtmlBody = string.Concat($"<em>{auditText}{message.Recipients.ConcatWithSeparator(", ")}</em><br><br>",
                message.HtmlBody),
        };
        mimeMessage.Body = auditBuilder.ToMessageBody();

        _ = EmailMessageAsync(mimeMessage, Settings, token).ConfigureAwait(false);
    }

    private static async Task EmailMessageAsync(MimeMessage emailMessage, EmailServiceSettings settings,
        CancellationToken token)
    {
        if (!Enum.TryParse(settings.SecureSocketOption, out SecureSocketOptions secureSocketOption))
            secureSocketOption = SecureSocketOptions.Auto;
        using var client = new SmtpClient();
        await client.ConnectAsync(settings.SmtpHost, settings.SmtpPort, secureSocketOption, token)
            .ConfigureAwait(false);
        await client.SendAsync(emailMessage, token).ConfigureAwait(false);
        await client.DisconnectAsync(true, token).ConfigureAwait(false);
    }
}
