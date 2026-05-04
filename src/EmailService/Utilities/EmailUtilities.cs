using MailKit.Security;
using MimeKit;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace GaEpd.EmailService.Utilities;

internal static class EmailUtilities
{
    public static MimeMessage CreateMimeMessage(Message message)
    {
        var mimeMessage = new MimeMessage();

        mimeMessage.From.Add(new MailboxAddress(message.SenderName, message.SenderEmail!));
        mimeMessage.Subject = message.Subject;
        mimeMessage.To.AddRange(message.Recipients.Select(ParseAddress));
        mimeMessage.Cc.AddRange(message.CopyRecipients.Select(ParseAddress));
        mimeMessage.Body = new BodyBuilder { TextBody = message.TextBody, HtmlBody = message.HtmlBody }.ToMessageBody();

        return mimeMessage;
    }

    private static MailboxAddress ParseAddress(string address)
    {
        var mailAddress = new MailAddress(address);
        return new MailboxAddress(mailAddress.DisplayName, mailAddress.Address);
    }

    public static async Task SendEmailAsync(MimeMessage emailMessage, EmailServiceSettings settings,
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
