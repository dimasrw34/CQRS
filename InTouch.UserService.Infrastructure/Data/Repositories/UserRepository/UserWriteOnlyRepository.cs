using System;
using System.Threading.Tasks;
using InTouch.UserService.Core;
using InTouch.UserService.Domain;

namespace InTouch.Infrastructure.Data;

public sealed class UserWriteOnlyRepository (
    IDbConnectionFactory connectionFactory,
    IUnitOfWork unitOfWork) :
                BaseWriteOnlyRepository<User, Guid>(connectionFactory,unitOfWork), 
                IUserWriteOnlyRepository<User, Guid>
{
    
    public Task<bool> ExistByEmailAsync(Email email)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistByEmailAndIdAsync(Email email, Guid Id)
    {
        throw new NotImplementedException();
    }
}
