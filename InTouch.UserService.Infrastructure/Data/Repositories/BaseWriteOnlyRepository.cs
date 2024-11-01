using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace InTouch.Infrastructure.Data;

public abstract class BaseWriteOnlyRepository(IDbConnection connection) 
{
    public async Task ExecuteAsync(string sql, IDbTransaction transaction, object param = null)
        => await  connection.QueryAsync(sql, param,transaction);
    
    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null) =>
        await connection.QueryAsync<T>(sql, param);
        
    public async Task<T> QuerySingleAsync<T>(string sql, IDbTransaction transaction, object? param = null) =>
        await connection.QueryFirstOrDefaultAsync<T>(sql, param,transaction);
}