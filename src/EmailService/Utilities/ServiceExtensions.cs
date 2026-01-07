using Microsoft.Extensions.DependencyInjection;

namespace GaEpd.EmailService.Utilities;

public static class ServiceExtensions
{
    public static IServiceCollection AddEmailService(this IServiceCollection services) =>
        services.AddTransient<IEmailService, DefaultEmailService>();
}
