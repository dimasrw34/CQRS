using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using FluentValidation;
using InTouch.UserService.Core;
using MediatR;

namespace InTouch.UserService.Application;

public class DeletePermissionCommandHandler(
    IValidator<DeletePermissionCommand> validator,
    IEventStoreRepository eventStoreRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    CancellationToken cancellationToken = default) 
    : IRequestHandler<DeletePermissionCommand, Result>
{
    public Task<Result> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}
