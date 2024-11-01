using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using InTouch.UserService.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using Polly;
using Polly.Retry;

namespace InTouch.UserService.Query.Data;

public sealed class NoSqlDbContext : ISynchronizedDb, IReadDbContext
{
    #region Constructors

    private const string DatabaseName = "Users";
    private const int RetryCount = 2;

    private static readonly ReplaceOptions DefaultReplaceOptions = new()
    {
        IsUpsert = true
    };

    private static readonly CreateIndexOptions DefaultCreateIndexOptions = new()
    {
        Unique = true,
        Sparse = true
    };

    private readonly IMongoDatabase _database;
    private readonly ILogger<NoSqlDbContext> _logger;
    private readonly AsyncRetryPolicy _mongoRetryPolicy;

    public NoSqlDbContext(IOptions<ConnectionString> options, ILogger<NoSqlDbContext> logger)
    {
        ConnectionString = "mongodb://192.168.1.116:27017";

        var mongoClient = new MongoClient("mongodb://192.168.1.116:27017");
        _database = mongoClient.GetDatabase(DatabaseName);
        _logger = logger;
        _mongoRetryPolicy = CreateRetryPolicy(logger);
    }
    
    private static AsyncRetryPolicy CreateRetryPolicy(ILogger logger)
    {
        return Policy
            .Handle<MongoException>()
            .WaitAndRetryAsync(RetryCount, SleepDurationProvider);
    
        void OnRetry(Exception ex, TimeSpan _) =>
            logger.LogError(ex, "An unexpected exception occurred while saving to MongoDB: {Message}", ex.Message);
    
        TimeSpan SleepDurationProvider(int retryAttempt)
        {
            // Повтор с джиттером
            // Хорошо известная стратегия повтора — экспоненциальная задержка, позволяющая выполнять повторы сначала быстро,
            // но затем с постепенно увеличивающимися интервалами: например, через 2, 4, 8, 15, затем 30 секунд.
            // REF: https://github.com/App-vNext/Polly/wiki/Retry-with-jitter#simple-jitter
            var sleepDuration =
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(Random.Shared.Next(0, 1000));
    
            logger.LogWarning("----- MongoDB: Повтор #{Count} задержка {Delay}", retryAttempt, sleepDuration);
    
            return sleepDuration;
        }
    }
    
    #endregion Constructors

    #region ISynchrinizedDB
    public async Task UpsertAsync<TQueryModel>(TQueryModel queryModel, Expression<Func<TQueryModel, bool>> upsertFilter) 
        where TQueryModel : IQueryModel
    {
        var collection = GetCollection<TQueryModel>();

        await _mongoRetryPolicy.ExecuteAsync(async () =>
            await collection.ReplaceOneAsync(upsertFilter, queryModel, DefaultReplaceOptions));
    }

    public async Task DeleteAsync<TQueryModel>(Expression<Func<TQueryModel, bool>> deleteFilter) 
        where TQueryModel : IQueryModel
    {
        var collecton = GetCollection<TQueryModel>();
        await _mongoRetryPolicy.ExecuteAsync(async () =>  await collecton.DeleteOneAsync(deleteFilter));
    }
    #endregion

    #region IReadDbContext

    public string ConnectionString { get; }

    public IMongoCollection<TQueryModel> GetCollection<TQueryModel>() where TQueryModel : IQueryModel =>
        _database.GetCollection<TQueryModel>(typeof(TQueryModel).Name);

    public async Task CreateCollectionAsync()
    {
        using var asyncCursor = await _database.ListCollectionNamesAsync();
        var collections = await asyncCursor.ToListAsync();

        foreach (var collectionName in GetCollectionNamesFromAssembly())
        {
            if (!collections.Exists(db => db.Equals(collectionName, StringComparison.CurrentCultureIgnoreCase)))
            {
                _logger.LogInformation("----- MongoDB: создана Collection {Name}.", collectionName);

                await _database.CreateCollectionAsync(collectionName, new CreateCollectionOptions
                {
                    ValidationLevel = DocumentValidationLevel.Strict
                });
            }
            else
            {
                _logger.LogInformation("----- MongoDB: {Name} коллекция существует.", collectionName);
            }
        }

        await CreateIndexAsync();
    }

    public async Task CreateIndexAsync()
    {
        _logger.LogInformation("------- MongoDB: создание индекса...");
        var indexDefinition = Builders<UserQueryModel>.IndexKeys.Ascending(model => model.Email);

        var indexModel = new CreateIndexModel<UserQueryModel>(indexDefinition, DefaultCreateIndexOptions);

        var collection = GetCollection<UserQueryModel>();
        var indexName = await collection.Indexes.CreateOneAsync(indexModel);
        _logger.LogInformation("------ MongoDB: индекс создан - {indexName}", indexName);
    }

    private static List<string> GetCollectionNamesFromAssembly() =>
        Assembly
            .GetExecutingAssembly()
            .GetAllTypesOf<IQueryModel>()
            .Select(impl => impl.Name)
            .Distinct()
            .ToList();
    #endregion IReadDbContext
}