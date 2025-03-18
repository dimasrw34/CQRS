using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using InTouch.UserService.Core;
using Npgsql;

namespace InTouch.Infrastructure.Data;

public class BaseWriteOnlyRepository<TEntity, TKey> (
    IDbConnectionFactory connectionFactory,
    IUnitOfWork unitOfWork)
    : IWriteOnlyRepository<TEntity,TKey>
    where TEntity : IEntity<TKey> 
    where TKey : IEquatable<TKey>
{
    protected readonly IDbConnectionFactory _connectionFactory = connectionFactory;
    protected readonly IUnitOfWork UnitOfWork = unitOfWork;
    
    private readonly Dictionary<string, PropertyInfo> ColumnMappings = PropertyHelper.GetColumnMappings(typeof(TEntity));
    protected virtual string TableName => typeof(TEntity).Name.ToLower();
    
    
    
    
    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var connection = await _connectionFactory.CreateOpenConnectionAsync(default);
        var sql = $"SELECT {string.Join(", ", PropertyHelper.GetColumnNames(typeof(TEntity)))} FROM {TableName}";
        
        return await connection.QueryAsync<TEntity>(
            sql,
            param: null,
            transaction: UnitOfWork.Transaction,
            commandTimeout: null,
            commandType: null
        );
    }

    public async Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        using var connection = await _connectionFactory.CreateOpenConnectionAsync(default);
        var sql = $"SELECT {string.Join(", ", PropertyHelper.GetColumnNames(typeof(TEntity)))} FROM {TableName} WHERE \"id\" = @id";
        
        return await connection.QueryFirstOrDefaultAsync<TEntity>(
            sql,
            param: new { id },
            transaction: UnitOfWork.Transaction,
            commandTimeout: null,
            commandType: null
        );
    }

    public async Task<Guid> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await using var connection = (NpgsqlConnection) await _connectionFactory.CreateOpenConnectionAsync(default);
        connection.EnlistTransaction(Transaction.Current);
        //NB: первым параметром засовываем id, чтобы всегда быть уверенным, что первый в коллекции
        var properties = ColumnMappings.Values.Where(p => p.Name == "id").ToList();
        properties.AddRange(ColumnMappings.Values.Where(p => p.Name != "id").ToList());
        
        
        var columns = string.Join(", ", properties.Select(p => $"\"{p.Name}\""));
        var values = string.Join(", ", properties.Select((p, i) => $"@param{i}"));

        var sql = $"INSERT INTO {TableName} + s ({columns}) VALUES ({values}) ;";

        var parameters = new DynamicParameters();
        /*
        for (int i = 0; i < properties.Count; i++)
        {
            //parameters.Add($"param{i}", properties[i].GetValue(entity), DbTypeMapper.GetDbType(properties[i].GetType() ?? typeof(object)),ParameterDirection.InputOutput);
            parameters.Add($"param{i}", properties[i].GetValue(entity));
        }
*/

         
        
        //await connection.ExecuteAsync(sql, parameters, UnitOfWork.Transaction);
        await connection.ExecuteAsync(@"INSERT INTO public.users (id, login, password, name, lastname, email, phone) 
                                        VALUES (@userid,  @login, @password,@name, @lastname, @email, @phone);", 
            new { userid =Guid.NewGuid(),
                login = "sfdsdf",
                password = "fdfs",
                name = "dfsdfsd",
                lastname = "sdfsdf",
                email = "dfsdf@fsdf.com",
                phone = "77777777",
            }, 
            UnitOfWork.Transaction);
        
        //return await Task.FromResult((TKey)Convert.ChangeType(Guid.NewGuid(), typeof(TKey)));
        return Guid.NewGuid();
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        using var connection = await _connectionFactory.CreateOpenConnectionAsync(default);
        var properties = ColumnMappings.Values.Where(p => p.Name != "id").ToList();
        var sets = string.Join(", ", properties.Select(p => $"\"{p.Name}\" = @{p.Name}"));

        var sql = $"UPDATE {TableName} SET {sets} WHERE \"id\" = @id RETURNING *";

        var parameters = new DynamicParameters();
        foreach (var property in properties)
        {
            parameters.Add(property.Name, property.GetValue(entity));
        }
        parameters.Add("id", ColumnMappings["id"].GetValue(entity));

        await connection.ExecuteAsync(sql, parameters, UnitOfWork.Transaction);
    }

    public async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        using var connection = await _connectionFactory.CreateOpenConnectionAsync(default);
        var sql = $"DELETE FROM {TableName} WHERE \"id\" = @id";

        await connection.ExecuteAsync(sql, new { id }, UnitOfWork.Transaction);
    }
}

