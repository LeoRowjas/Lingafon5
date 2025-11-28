using Lingafon.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace Lingafon.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<ApplicationProfile>());
        return services;
    }
}

