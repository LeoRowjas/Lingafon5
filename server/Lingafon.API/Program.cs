using System.Text;
using Microsoft.ML.OnnxRuntime;
using KokoroSharp;
using Lingafon.Api.Middleware;
using Lingafon.API.WebSockets;
using Lingafon.Application;
using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SessionOptions = Microsoft.ML.OnnxRuntime.SessionOptions;

namespace Lingafon.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins("http://localhost:3000", "http://localhost:5173", "http://frontend:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        builder.Services.AddDbContext<LingafonDbContext>(option=>
        {
            option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Lingafon API",
                Version = "v1",
                Description = "API for Lingafon cabinet, team 5.",
            });
          
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Введите токен в формате: Bearer {токен}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"] ?? string.Empty))
                };
            });
        builder.Services.AddAuthorization();
        
        builder.Services.AddApplicationServices(builder.Configuration); //Register Application layer services
        builder.Services.AddInfrastructureServices(builder.Configuration); //Register Infrastructure layer services
        builder.Services.AddScoped<WebSocketHandler>(); //Register service from API layer

        var app = builder.Build();
        
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<LingafonDbContext>();
            dbContext.Database.Migrate();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        
        app.UseCors("AllowFrontend");
        
        app.UseHttpsRedirection();

        app.MapControllers();
        
        app.UseWebSockets();
        // WebSocket endpoint
        app.Map("/ws", async context =>
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                using var scope = context.RequestServices.CreateScope();
                var webSocketHandler = scope.ServiceProvider.GetRequiredService<WebSocketHandler>();
                var socket = await context.WebSockets.AcceptWebSocketAsync();
                await webSocketHandler.HandleAsync(context, socket);
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        });
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.Run();
    }
}