using System.Net.Http.Headers;
using Lingafon.Core.Entities;
using Lingafon.Core.Interfaces.Repositories;
using Lingafon.Core.Interfaces.Services;
using Lingafon.Infrastructure.Persistence.Repositories;
using Lingafon.Infrastructure.Services;
using Lingafon.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace Lingafon.Infrastructure;
public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var modelPath = Path.Combine(AppContext.BaseDirectory, "ggml-tiny.bin");

        if (!File.Exists(modelPath))
        {
            Console.WriteLine($"[WARNING] Whisper model not found at {modelPath}. Speech-to-text will not work until the model is added.");
        }
        else
        {
            Console.WriteLine($"Using Whisper model from: {modelPath}");
        }
        
        services.AddSingleton(modelPath);

        services.Configure<S3Settings>(configuration.GetSection("S3Settings"));
        services.Configure<OpenAiSettings>(configuration.GetSection("OpenAI"));
        
        services.AddHttpClient<IAiChatService, OllamaChatService>(client =>
        {
            client.BaseAddress = new Uri("http://ollama:11434/");
            client.Timeout = TimeSpan.FromSeconds(300);
        });

        services.AddScoped<IAiSpeechService>(provider =>
        {
            var fileStorage = provider.GetRequiredService<IFileStorageService>();
            var s3Settings = provider.GetRequiredService<IOptions<S3Settings>>();
            var model = provider.GetRequiredService<string>();
            return new OpenAiSpeechService(fileStorage, s3Settings, model);
        });
        
        services.AddScoped<IAssignmentRepository, AssignmentRepository>();
        services.AddScoped<IAssignmentResultRepository, AssignmentResultRepository>();
        services.AddScoped<IDialogRepository, DialogRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IInviteLinkRepository, InviteLinkRepository>();
        services.AddScoped<ITeacherStudentRepository, TeacherStudentRepository>();

        services.AddScoped<IFileStorageService, S3FileStorageService>();
        
        services.AddScoped<PasswordHasher<User>>();
        services.AddScoped<IPasswordHasher, PasswordHashService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}