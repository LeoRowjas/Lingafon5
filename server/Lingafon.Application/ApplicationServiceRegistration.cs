using Lingafon.Application.Interfaces.Services;
using Lingafon.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lingafon.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(_ => { }, typeof(ApplicationServiceRegistration).Assembly);
        
        services.AddScoped<IAssignmentService, AssignmentService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDialogService, DialogService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IAssignmentResultService, AssignmentResultService>();
        
        return services;
    }
}



