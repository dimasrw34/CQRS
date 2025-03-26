using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using FluentValidation;
using InTouch.UserService.Core;
using MediatR;

namespace InTouch.UserService.Application;

public sealed class DeleteRolePermissionBindingCommandHandler(
    IValidator<DeleteRolePermissionBindingCommand> validator,
    IEventStoreRepository eventStoreRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    CancellationToken cancellationToken = default
) : IRequestHandler<DeleteRolePermissionBindingCommand, Result>
{
    public Task<Result> Handle(DeleteRolePermissionBindingCommand request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}
