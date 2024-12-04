namespace GaEpd.EmailService;

public class EmailServiceSettings
{
    public bool EnableEmail { get; init; }
    public string SmtpHost { get; init; } = string.Empty;
    public int SmtpPort { get; init; }
    public string SecureSocketOption { get; init; } = string.Empty;
    public string DefaultSenderName { get; init; } = string.Empty;
    public string DefaultSenderEmail { get; init; } = string.Empty;
    public bool EnableEmailAuditing { get; init; }
    public List<string> AuditEmailRecipients { get; init; } = null!;
    public bool EnableEmailLog { get; init; }
}
