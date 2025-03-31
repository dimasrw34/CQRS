using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using InTouch.UserService.Core;
using Npgsql;

namespace InTouch.UserService.Infrastructure.Data;

/// <summary>
/// Базовый репозиторий
/// </summary>
public abstract class BaseRepository <TEntity,TKey>(
    IDbConnectionFactory connectionFactory,
    IUnitOfWork unitOfWork) 
    where TEntity : class, IEntity<TKey> 
    where TKey : IEquatable<TKey>
{
    protected readonly IDbConnectionFactory _connectionFactory = connectionFactory;
    protected readonly IUnitOfWork UnitOfWork = unitOfWork;

    protected virtual string TableName => typeof(TEntity).Name.ToLower();
    private protected NpgsqlConnection _npgsqlConnection { get; set;}

    public async Task ExecuteAsync(string sql, object param)
    {
        try
        {
            _npgsqlConnection = _connectionFactory.GetConnection;
            await _npgsqlConnection.QueryAsync(sql, param);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Ошибка ExecuteAsync");
            throw;
        }
    }

  public async IAsyncEnumerable<T> Query<T>(string sql, object param = null)
  {
      _npgsqlConnection = _connectionFactory.GetConnection;
      var reader = await _npgsqlConnection.ExecuteReaderAsync(sql, param);
      var rowParser = reader.GetRowParser<T>();

      while (await reader.ReadAsync())
      {
          yield return rowParser(reader);
      }
  }

  public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
  {
      try
      {
          _npgsqlConnection = _connectionFactory.GetConnection;
          return await _npgsqlConnection.QueryAsync<T>(sql, param);
      }
      catch (Exception ex)
      {
          //_logger.LogError(ex, "Ошибка QueryAsync");
          throw;
      }
  }
  private protected async Task<T> QuerySingleAsync<T>(string sql, object param = null)
  {
      try
      {
          var connection = _connectionFactory.GetConnection;
          return await connection.QueryFirstAsync<T>(sql, param);
      }
      catch (Exception ex)
      {
          //_logger.LogError(ex, "Ошибка QuerySingleAsync");
          throw;
      }
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

     public async Task Create(TEntity entity)
    {
        var paramsics = GetCreateParams(entity);
        
        await _npgsqlConnection.ExecuteAsync(paramsics.Item1, paramsics.Item2, UnitOfWork.Transaction);
    }
}
