using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InTouch.UserService.Query.Data.Repositories.Abstractions;
using MongoDB.Driver;

namespace InTouch.UserService.Query.Data.Repositories;

public class UserReadOnlyRepository (IReadDbContext readDbContext) :
    BaseReadOnlyRepository<UserQueryModel, Guid> (readDbContext),
    IUserReadOnlyRepository
{
    public async Task<IEnumerable<UserQueryModel>> GetAllAsync()
    {
        var sort = Builders<UserQueryModel>.Sort
            .Ascending(user => user.Email);

        var findOptions = new FindOptions<UserQueryModel>
        {
            Sort = sort
        };

        using var asyncCursor = await Collection.FindAsync(Builders<UserQueryModel>.Filter.Empty, findOptions);
        return await asyncCursor.ToListAsync();
    }
}