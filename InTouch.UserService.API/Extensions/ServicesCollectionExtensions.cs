using System;
using System.IO;
using System.Reflection;
using InTouch.Infrastructure;
using InTouch.UserService.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace InTouch.UserService;

internal static class ServicesCollectionExtensions
{
    private const string RedisInstanceName = "master";
    
    public static IServiceCollection AddCacheService(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetOptions<ConnectionOptions>();
        
        services.AddDistributedCacheService();
        services.AddStackExchangeRedisCache(redisOptions =>
        {
            redisOptions.InstanceName = RedisInstanceName;
            redisOptions.Configuration = options.CacheConnection;
        });
        return services;
    }

    /// <summary>
    /// Настройка swagger'a
    /// </summary>
    /// <param name="services"></param>
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen (swaggerOption =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            swaggerOption.IncludeXmlComments(xmlPath);

            swaggerOption.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {       
                In = ParameterLocation.Header,
                Description = "Авторизация",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            
            swaggerOption.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                new string[]{}
                }
            });

            swaggerOption.SwaggerDoc("v1", new OpenApiInfo 
            {
                Version = "v0.0.1" ,
                Title = "InTouch.UserService API",
                Description = "Сервис безопасности",
                Contact = new OpenApiContact
                {
                    Name = "Dmitry Boyarski" ,
                    Email = "boyarskidm@yandex.ru"
                },
                License = new OpenApiLicense
                {
                    Name =  "MIT License"
                }
            });

            swaggerOption.MapType<DateOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date"
            });
            
            swaggerOption.MapType<TimeOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "time",
                Example = OpenApiAnyFactory.CreateFromJson("\"13:45:45.0000000\"")
            });
        });
    }
}