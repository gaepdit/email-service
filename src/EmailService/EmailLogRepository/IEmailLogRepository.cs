namespace GaEpd.EmailService.EmailLogRepository;

public interface IEmailLogRepository : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Saves a copy of an <see cref="Message"/> to the configured repository.
    /// </summary>
    /// <param name="message">The Message to log.</param>
    /// <param name="token"><see cref="T:System.Threading.CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    Task InsertAsync(Message message, CancellationToken token = default);
}
