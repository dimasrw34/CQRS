using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using FluentValidation;
using InTouch.UserService.Core;
using MediatR;
using InTouch.UserService.Infrastructure.Data;

namespace InTouch.UserService.Application;

public sealed class UpdateRoleCommandHandler(
    IValidator<UpdateRoleCommand> validator,
    IEventStoreRepository eventStoreRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    CancellationToken cancellationToken = default
) : IRequestHandler<UpdateRoleCommand, Result>
{
    public Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
