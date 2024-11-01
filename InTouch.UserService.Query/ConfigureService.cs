using System.Reflection;
using AutoMapper;
using InTouch.UserService.Query.Data;
using InTouch.UserService.Query.Data.Repositories;
using InTouch.UserService.Query.Data.Repositories.Abstractions;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using FluentValidation;

namespace InTouch.UserService.Query;

public static class ConfigureService
{
    /// <summary>
    /// Добавляет обработчики запросов в коллекцию сервисов.
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return services
             .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly))
            .AddSingleton<IMapper>(new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(assembly))))
            .AddValidatorsFromAssembly(assembly);
        
        
    }
    
    /// <summary>
    /// Добавляет контекст базы данных чтения в коллекцию сервисов.
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    public static IServiceCollection AddReadDbContext(this IServiceCollection services)
    {
        services
            .AddSingleton<ISynchronizedDb, NoSqlDbContext>()
            .AddSingleton<IReadDbContext, NoSqlDbContext>()
            .AddSingleton<NoSqlDbContext>();

        ConfigureMongoDb();

        return services;
    }

    /// <summary>
    /// Добавляет репозитории, доступные только для чтения, в коллекцию служб.
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <returns></returns>
    public static IServiceCollection AddReadOnlyRepositories(this IServiceCollection services) =>
        services.AddScoped<IUserReadOnlyRepository, UserReadOnlyRepository>();
    
    
    /// <summary>
    /// Конфигурация MongoDB и mappnigs.
    /// </summary>
    private static void ConfigureMongoDb()
    {
        try
        {
            // 1: Настройка сериализатора для типа Guid.
            BsonSerializer.TryRegisterSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));

            // 2: Настройте соглашения, которые будут применяться ко всем сопоставлениям.
            // REF: https://mongodb.github.io/mongo-csharp-driver/2.0/reference/bson/mapping/conventions/
            ConventionRegistry.Register("Conventions",
                new ConventionPack
                {
                    new CamelCaseElementNameConvention(), // Преобразовать имена элементов в Camel Case нотацию
                    new EnumRepresentationConvention(BsonType.String), // Сериализует перечисления как тип string
                    new IgnoreExtraElementsConvention(true), // Игнорировать лишние элементы при десериализации
                    new IgnoreIfNullConvention(true) // Игнорировать нулевые значения при сериализации
                }, _ => true);

            // Step 3: Регистрация конфигурации сопоставлений.
            // Рекомендуется настроить конфигурацию до инициализации соединения с MongoDb
            // REF: https://mongodb.github.io/mongo-csharp-driver/2.0/reference/bson/mapping/
            new UserMap().Configure(); // Конфигурация для User класс
        }
        catch
        {
            // ничего Ж)
        }
    }
}