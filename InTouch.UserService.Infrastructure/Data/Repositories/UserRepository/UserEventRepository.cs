using System.Data;
using System.Threading.Tasks;
using InTouch.UserService.Core;

namespace InTouch.Infrastructure.Data;

public sealed class UserEventRepository(IDbContext context) 
    : BaseWriteOnlyRepository(context.Connection),
        IEventStoreRepository
{
    private readonly IDbConnection _connection = context.Connection;
    private readonly IDbTransaction _transaction = context.Transaction;
    public async Task StoreAsync(EventStore? eventStore) =>
        await ExecuteAsync
            (@"INSERT INTO public.eventstores (id, datastamp, messagetype, aggregateid, createdat) 
                            VALUES (@eventdid_, @datastamp_, @messagetype_, @aggregateid_, @createdat_);",
                        _transaction,
            new { eventdid_= eventStore.Id,
                        datastamp_ = eventStore.Data,
                        messagetype_ = eventStore.MessageType,
                        aggregateid_ = eventStore.AggregateID,
                        createdat_ = eventStore.OccuredOn
                        }
            );
}