namespace GaEpd.EmailService;

public interface IEmailService
{
    Task SendEmailAsync(Message message, CancellationToken token = default);
}
