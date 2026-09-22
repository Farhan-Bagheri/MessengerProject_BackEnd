using Identity.Facade.User;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Facade;

public class FacadeBootstrapper
{
    public static void RegisterDependency(IServiceCollection services)
    {
        services.AddScoped<IUserFacade, UserFacade>();
    }
}
