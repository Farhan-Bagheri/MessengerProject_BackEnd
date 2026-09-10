using Identity.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Configuration;

public static class Bootstrapper
{
    public static void Configure(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            ArgumentException.ThrowIfNullOrEmpty(nameof(connectionString));

        InfrastructureBootstrapper.RegisterDependency(services, connectionString);
    }
}
