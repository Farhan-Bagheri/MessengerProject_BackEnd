using Microsoft.Extensions.DependencyInjection;
using Shop.Infrastructure;

namespace Shop.Configuration;

public static class Bootstrapper
{
    public static void Configure(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            ArgumentException.ThrowIfNullOrEmpty(nameof(connectionString));

        InfrastructureBootstrapper.RegisterDependency(services, connectionString);
    }
}
