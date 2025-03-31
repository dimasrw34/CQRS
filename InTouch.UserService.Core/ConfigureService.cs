using System.Reflection;
using System.Text.Json;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;

namespace InTouch.UserService.Core;

public static class ConfigureService
{
    public static IServiceCollection AddResponseMediatr(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        return services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.NotificationPublisher = new TaskWhenAllPublisher();
        });
    }

    public static IServiceCollection ConfigureAppSettings(this IServiceCollection services) =>
        services
            .AddOptionsWithValidation<ConnectionOptions>()
            .AddOptionsWithValidation<CacheOptions>();
    
            //.AddOptionsWithValidation<ConnectionOptions>()
            /// <summary>
            /// Добавляет опции с проверкой в коллекцию услуг.
            /// </summary>
            /// <typeparam name="TOptions">The type of options to add.</typeparam>
            /// <param name="services">The service collection.</param>
            private static IServiceCollection AddOptionsWithValidation<TOptions>(this IServiceCollection services)
                where TOptions : class, IAppOptions
            {
                return services
                    .AddOptions<TOptions>()
                    .BindConfiguration(TOptions.ConfigSectionPath, binderOptions => binderOptions.BindNonPublicProperties = true)
                    .ValidateDataAnnotations()
                    .ValidateOnStart()
                    .Services;
            }
            
}