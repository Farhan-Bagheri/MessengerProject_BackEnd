using Microsoft.Extensions.DependencyInjection;
using Shop.Facade.Category;
using Shop.Facade.Product;
using Shop.Facade.Store;

namespace Shop.Facade;

public class FacadeBootstrapper
{
    public static void RegisterDependency(IServiceCollection services)
    {
        services.AddScoped<IProductFacade, ProductFacade>();
        services.AddScoped<IStoreFacade, StoreFacade>();
        services.AddScoped<ICategoryFacade, CategoryFacade>();
    }
}
