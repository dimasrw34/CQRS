using System;
using System.Threading.Tasks;
using InTouch.UserService.Core;
using InTouch.UserService.Domain;
using InTouch.UserService.Infrastructure;

using System.Text;
using Npgsql.Replication;
using System.Runtime.InteropServices;
using Dapper;
using System.Linq;
using Ardalis.Result;
using System.Collections.Generic;
using System.Threading;


namespace InTouch.UserService.Infrastructure.Data;

public class UserWriteOnlyRepository <TEntity,TKey> (
    IDbConnectionFactory connectionFactory,
    IUnitOfWork unitOfWork)
: BaseWriteOnlyRepository <User, Guid> (connectionFactory, unitOfWork)
{
    private readonly  IDbConnectionFactory _connectionFactory = connectionFactory;
    private readonly  IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<bool> ExistByLoginAsync(ISpecification<User,Guid> specification)
    {
        var connection = _connectionFactory.GetConnection;
        var specificationResult = specification.ToSqlQueryString();
        
        var Result = await connection.ExecuteAsync(specificationResult.Item1, specificationResult.Item2, _unitOfWork.Transaction);
        return Result == 0 ? false : true; 
    }
}

/*
    public Task<bool> ExistByEmailAndIdAsync(Email email, Guid Id)
    {
        throw new NotImplementedException();
    }
*/
