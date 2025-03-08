using System;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

using InTouch.Infrastructure.Data;
using InTouch.UserService.Core;
using InTouch.UserService.Domain;

namespace InTouch.Infrastructure;

public static class ConfigureService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddRegisterTypeHandler(this IServiceCollection services)
    {
        SqlMapper.AddTypeHandler(new EmailTypeHandler());
        return services;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services
            .AddTransient<IDbConnectionFactory> (options =>
            {
                var builder = new NpgsqlConnectionStringBuilder(
                                                "Host=192.168.1.40;Database=secbase;Username=dima;Password=123;" + 
                                                "Persist Security Info=True;Application Name=userservice;Enlist=true");
                return new DbConnectionFactory(() =>
                {
                    var conn = new NpgsqlConnection(builder.ConnectionString);
                    conn.Open();
                    return conn;
                });
            })
            .AddScoped<IDbContext, DbContext>();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddWriteOnlyRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IUserWriteOnlyRepository<User, Guid>, UserWriteOnlyRepository>()
            .AddScoped<IEventStoreRepository,UserEventRepository>();
    }

    public static void AddDistributedCacheService(this IServiceCollection services) =>
        services.AddScoped<ICacheService, DistributedCashService>();
}