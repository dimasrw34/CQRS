using System;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using InTouch.Infrastructure.Data;
using InTouch.UserService.Core;
using InTouch.UserService.Domain;
using InTouch.UserService.Infrastructure.Authentification;
using InTouch.UserService.Infrastructure.Authorization;
using Npgsql;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

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

    public static IServiceCollection AddAuthentificationAndAuthorization (this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

        services.AddAuthentication(op =>
        {
            op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            op.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,op => 
        {
            op.RequireHttpsMetadata = true;
            op.SaveToken = true;
            op.TokenValidationParameters = new ()
            { 
               ValidateIssuer = false,
               ValidateAudience = false,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
            };
            op.Events = new JwtBearerEvents
            {
                OnMessageReceived = context => 
                {
                    context.Request.Cookies.TryGetValue("", out var accessToken);
                    context.Token = accessToken;
                    
                    return Task.CompletedTask;
                }
            };
        });


        //TODO: реализовать сервис получения Permissions из БД
        //services.AddScoped<IPermissionService, PermissionService>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider,JwtProvider>();
        
        services.AddAuthorization();
        
        return services;
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

