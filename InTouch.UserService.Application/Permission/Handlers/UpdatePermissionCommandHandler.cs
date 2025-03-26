using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using FluentValidation;
using InTouch.UserService.Core;
using MediatR;

namespace InTouch.UserService.Application;

public class UpdatePermissionCommandHandler(
    IValidator<UpdatePermissionCommand> validator,
    IEventStoreRepository eventStoreRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    CancellationToken cancellationToken = default) 
    : IRequestHandler<UpdatePermissionCommand, Result>
{
    public Task<Result> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}