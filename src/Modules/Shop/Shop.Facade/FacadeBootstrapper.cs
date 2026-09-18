using Microsoft.Extensions.DependencyInjection;
using Shop.Facade.Product;

namespace Shop.Facade;

public class FacadeBootstrapper
{
    public static void RegisterDependency(IServiceCollection services)
    {
        services.AddScoped<IProductFacade, ProductFacade>();
    }
}
