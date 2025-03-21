using System;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using InTouch.Infrastructure.Data;
using InTouch.UserService.Core;
using InTouch.UserService.Domain;
using Microsoft.AspNetCore.Http.Json;
using Npgsql;

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
        services.AddSingleton<IDbConnectionFactory>(sp =>
        {
            var dataSource = sp.GetService<NpgsqlDataSource>();
            return new DbConnectionFactory(() => dataSource);
        });
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder()
        {
            Host = "192.168.1.40",
            Database = "secbase",
            Username = "dima",
            //TODO: Реализовать получение из хранилища
            Password = "123",
            PersistSecurityInfo = true,
            ApplicationName = "userservice",
            Enlist = false
        };
        return services.AddNpgsqlDataSource(connectionStringBuilder.ToString());
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
            .AddScoped<IEventStoreRepository,UserEventRepository>()
            .AddScoped<IUnitOfWork,UnitOfWork>();
    }

    public static void AddDistributedCacheService(this IServiceCollection services) =>
        services.AddScoped<ICacheService, DistributedCashService>();
}

