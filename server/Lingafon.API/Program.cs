using Lingafon.Api.Middleware;
using Lingafon.Application;
using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Lingafon.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        //TODO: CORS
        //TODO: Authentication & Authorization BEARING

        builder.Services.AddDbContext<LingafonDbContext>(option=>
        {
            option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
        
        
        //TODO: EXCEPTION HANDLING MIDDLEWARE
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Lingafon API",
                Version = "v1",
                Description = "API for Lingafon cabinet, team 5.",
            });
        });
        builder.Services.AddOpenApi();
        
        builder.Services.AddApplicationServices(); //Register Application layer services
        builder.Services.AddInfrastructureServices(); //Register Infrastructure layer services

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.MapControllers();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        
        app.Run();
    }
}