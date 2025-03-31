using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using FluentValidation;
using InTouch.UserService.Core;
using MediatR;
using InTouch.UserService.Infrastructure.Data;

namespace InTouch.UserService.Application;

public sealed class DeleteRoleCommandHandler(
    IValidator<DeleteRoleCommand> validator,
    IEventStoreRepository eventStoreRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    CancellationToken cancellationToken = default
) : IRequestHandler<DeleteRoleCommand, Result>
{
    public Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}
