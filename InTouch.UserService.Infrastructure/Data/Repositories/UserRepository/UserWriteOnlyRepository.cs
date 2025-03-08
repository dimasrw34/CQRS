using System;
using System.Data;
using System.Threading.Tasks;
using InTouch.UserService.Domain;

namespace InTouch.Infrastructure.Data;

public sealed class UserWriteOnlyRepository (IDbContext context) :
                BaseWriteOnlyRepository(context.Connection), 
                IUserWriteOnlyRepository<User, Guid>
{
    private readonly IDbConnection _connection = context.Connection;
    private readonly IDbTransaction _transaction = context.Transaction;
    public async Task<Guid> AddAsync(User user) 
    {
                await ExecuteAsync(
                    UserScripts.GetScript(nameof(AddAsync)),
                    _transaction,
                    new { userid = user.Id,
                                email = user.Email.ToString(),
                                password = user.Password,
                                name = user.Name,
                                surname = user.Surname,
                                phone = user.Phone,
                                });
                return user.Id;
    }

    public async Task UpdateAsync(User user) =>
        await ExecuteAsync(
            UserScripts.GetScript(nameof(UpdateAsync)),
            _transaction,
            new
            {
                email =  user.Email.Address,
                id = user.Id
            });


    public async Task<User> GetByIdAsync(Guid id) =>
        await QuerySingleAsync<User>(
            UserScripts.GetScript(nameof(GetByIdAsync)),
            _transaction,
            new {id = id});
    
    public async Task<bool> ExistByEmailAsync(Email email) =>
        await QuerySingleAsync<bool>(
            UserScripts.GetScript(nameof(ExistByEmailAsync)),
                                 _transaction,
                        new {email = email.Address});
    
    public async Task<bool> ExistByEmailAndIdAsync(Email email, Guid currentId) =>
        await QuerySingleAsync<bool>(
            UserScripts.GetScript(nameof(ExistByEmailAndIdAsync)),
            _transaction,
            new
            {
                id = currentId,
                email = email.Address
            });

    public async Task DeleteAsync(Guid id) =>
        await ExecuteAsync(
            UserScripts.GetScript(nameof(DeleteAsync)),
                            _transaction,
                        new {id = id});
}
