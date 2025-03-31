using System;
using System.Threading.Tasks;
using Ardalis.Specification;
using InTouch.UserService.Core;
using InTouch.UserService.Domain;

namespace InTouch.UserService.Infrastructure.Data;

public interface IUserWriteOnlyRepository<TEntity, TKey> 
{
     Task<bool> ExistByLoginAsync(UserByLoginSpecification? specification);
        //Task<bool> ExistByEmailAndIdAsync(Email email, Guid Id);
}