using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using InTouch.UserService.Core;



namespace InTouch.UserService.Infrastructure.Data;

public class BaseWriteOnlyRepository<TEntity, TKey> (
    IDbConnectionFactory connectionFactory,
    IUnitOfWork unitOfWork)
    : IWriteOnlyRepository<TEntity,TKey>
    where TEntity : class, IEntity<TKey> 
    where TKey : IEquatable<TKey>
{
    private readonly  IDbConnectionFactory _connectionFactory = connectionFactory;
    private readonly  IUnitOfWork _unitOfWork = unitOfWork;
    private readonly Dictionary<string, PropertyInfo> ColumnMappings = PropertyHelper.GetColumnMappings(typeof(TEntity));

    protected virtual string TableName => typeof(TEntity).Name.ToLower();
    public async Task<TKey> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var connection = _connectionFactory.GetConnection;
        var paramsics = GetCreateParams(entity);
        
        await connection.ExecuteAsync(paramsics.Item1, paramsics.Item2, _unitOfWork.Transaction);
        return await Task.FromResult((TKey)Convert.ChangeType(entity.Id, typeof(TKey)));
    }

    private (string, DynamicParameters) GetCreateParams(TEntity entity)
    {
        // Получаем все свойства сущности за исключением события
        var allProperties = typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.Name != "DomainEvents")
            .ToList();
        
        // Формируем список колонок с учётом кавычек для PostgreSQL
        var columns = string.Join(", ", allProperties.Select(p => $"\"{p.Name}\"")).ToLower();
    
        // Формируем параметры, добавляя @param0 в начало
        var values = string.Join(", ", allProperties.Select((p, i) => "@param" + i));
        

        var sql = $"INSERT INTO {TableName}s ({columns}) VALUES ({values}) ;";
        
        var parameters = new DynamicParameters();
        int paramIndex = 0;
        foreach (var property in allProperties)
        {
            var propertyValue = property.GetValue(entity);
            
            // Если свойство реализует IValueObject, используем его значение
            if (propertyValue is IValueObject valueObject)
            {
                parameters.Add($"param{paramIndex}", valueObject.ToString());
            }
            else
            {
                parameters.Add($"param{paramIndex}", propertyValue);
            }
            paramIndex++;
        }
        return (sql, parameters);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var connection =_connectionFactory.GetConnection;
        var properties = ColumnMappings.Values.Where(p => p.Name != "id").ToList();
        var sets = string.Join(", ", properties.Select(p => $"\"{p.Name}\" = @{p.Name}"));

        var sql = $"UPDATE {TableName} SET {sets} WHERE \"id\" = @id RETURNING *";

        var parameters = new DynamicParameters();
        foreach (var property in properties)
        {
            parameters.Add(property.Name, property.GetValue(entity));
        }
        parameters.Add("id", ColumnMappings["id"].GetValue(entity));

        await connection.ExecuteAsync(sql, parameters, _unitOfWork.Transaction);
    }

    public async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var sql = $"DELETE FROM {TableName} WHERE \"id\" = @id";

        await _connectionFactory.GetConnection.ExecuteAsync(sql, new { id },_unitOfWork.Transaction);
    }

    public async Task StoreAsync(EventStore? eventStore, CancellationToken cancellationToken= default) =>
        await _connectionFactory.GetConnection.ExecuteAsync 
        (@"INSERT INTO public.eventstores (id, datastamp, messagetype, aggregateid, createdat) 
                            VALUES (@eventdid_, @datastamp_, @messagetype_, @aggregateid_, @createdat_);",
            new { eventdid_= eventStore.Id,
                datastamp_ = eventStore.Data,
                messagetype_ = eventStore.MessageType,
                aggregateid_ = eventStore.AggregateID,
                createdat_ = eventStore.OccuredOn
            }
            ,_unitOfWork.Transaction
        );
}