using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace InTouch.UserService.Core;

public static class ServiceProviderExtensions
{
    /// <summary>
    /// Получите варианты у поставщика услуг.
    /// </summary>
    /// <typeparam name="TOptions">The options type.</typeparam>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns>The options.</returns>
    public static TOptions GetOptions<TOptions>(this IServiceProvider serviceProvider)
        where TOptions : class, IAppOptions =>
        serviceProvider.GetService<IOptions<TOptions>>()?.Value;   
}