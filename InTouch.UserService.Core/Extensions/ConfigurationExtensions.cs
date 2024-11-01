using Microsoft.Extensions.Configuration;

namespace InTouch.UserService.Core;

public static class ConfigurationExtensions
{
    /// <summary>
    /// Получить параметры из объекта IConfiguration.
    /// </summary>
    /// <param name="configuration">he IConfiguration object.</param>
    /// <typeparam name="TOptions">Тип опций для извлечения.</typeparam>
    /// <returns>Объект параметров</returns>
    public static TOptions GetOptions<TOptions>(this IConfiguration configuration)
        where TOptions : class, IAppOptions
    {
        return configuration
            .GetRequiredSection(TOptions.ConfigSectionPath)
            .Get<TOptions>(options => options.BindNonPublicProperties = true);
    }
}