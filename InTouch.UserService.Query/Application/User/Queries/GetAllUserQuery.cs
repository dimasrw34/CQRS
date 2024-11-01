using System.Collections.Generic;
using Ardalis.Result;
using InTouch.UserService.Query.Data.Repositories.Abstractions;
using MediatR;

namespace InTouch.UserService.Query;

public class GetAllUserQuery  : IRequest<Result<IEnumerable<UserQueryModel>>>;