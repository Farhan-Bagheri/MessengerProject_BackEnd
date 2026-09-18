using Microsoft.Extensions.DependencyInjection;
using Shop.Facade;
using Shop.Infrastructure;

namespace Shop.Configuration;

public static class Bootstrapper
{
    public static void Configure(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            ArgumentException.ThrowIfNullOrEmpty(nameof(connectionString));

        FacadeBootstrapper.RegisterDependency(services);
        InfrastructureBootstrapper.RegisterDependency(services, connectionString);
    }
}
