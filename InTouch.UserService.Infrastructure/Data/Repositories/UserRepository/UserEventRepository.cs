using System;
using System.Threading.Tasks;
using System.Transactions;
using InTouch.UserService.Core;
using InTouch.UserService.Domain;
using Npgsql;
using Dapper;

namespace InTouch.Infrastructure.Data;

public sealed class UserEventRepository(
    IDbConnectionFactory connectionFactory,
    IUnitOfWork unitOfWork) 
    : BaseWriteOnlyRepository<User, Guid>(connectionFactory,unitOfWork),
    IEventStoreRepository
{
    internal readonly IUnitOfWork _unitOfWork = unitOfWork;
    internal readonly IDbConnectionFactory _connectionFactory = connectionFactory;
    public async Task StoreAsync(EventStore eventStore)
    {
        await using var connection = (NpgsqlConnection) await _connectionFactory.CreateOpenConnectionAsync(default);
        connection.EnlistTransaction(Transaction.Current);
        
        await connection.ExecuteAsync 
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
}


