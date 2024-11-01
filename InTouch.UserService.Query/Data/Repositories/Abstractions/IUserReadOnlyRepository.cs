using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTouch.UserService.Query.Data.Repositories.Abstractions;

public interface IUserReadOnlyRepository : IReadOnlyRepository<UserQueryModel, Guid>
{
    Task<IEnumerable<UserQueryModel>> GetAllAsync();
}