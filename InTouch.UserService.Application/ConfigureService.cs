using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
    
namespace InTouch.UserService.Application;

public static class ConfigureService
{
    /// <summary>
    /// Добавляет command handlers в service collection
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns></returns>
    public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        return services
            .AddValidatorsFromAssembly(assembly)
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly)
            .AddOpenBehavior(typeof(RequestLogginingPipeLineBehavior<,>)));
    }
}