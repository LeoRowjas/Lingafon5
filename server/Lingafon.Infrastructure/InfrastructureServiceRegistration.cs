using Lingafon.Core.Entities;
using Lingafon.Core.Interfaces.Repositories;
using Lingafon.Core.Interfaces.Services;
using Lingafon.Infrastructure.Persistence.Repositories;
using Lingafon.Infrastructure.Services;
using Lingafon.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lingafon.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAssignmentRepository, AssignmentRepository>();
        services.AddScoped<IAssignmentResultRepository, AssignmentResultRepository>();
        services.AddScoped<IDialogRepository, DialogRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.Configure<S3Settings>(configuration.GetSection("S3Settings"));
        services.AddScoped<IFileStorageService, S3FileStorageService>();

        services.AddScoped<PasswordHasher<User>>();
        services.AddScoped<IPasswordHasher, PasswordHashService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        
        return services;
    }
}