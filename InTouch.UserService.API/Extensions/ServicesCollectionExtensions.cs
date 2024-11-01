using InTouch.Infrastructure;
using InTouch.UserService.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
}