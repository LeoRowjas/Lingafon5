using Lingafon.Application.Interfaces.Services;
using Lingafon.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Lingafon.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(_ => { }, typeof(ApplicationServiceRegistration).Assembly);
        
        services.AddScoped<IAssignmentService, AssignmentService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDialogService, DialogService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IAssignmentResultService, AssignmentResultService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IInviteLinkService, InviteLinkService>();
        
        services.Configure<StorageSettings>(configuration.GetSection("S3Settings"));
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<StorageSettings>>().Value);
        
        return services;
    }
}
