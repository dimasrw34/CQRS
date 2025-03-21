using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using FluentValidation;
using Ardalis.Result.FluentValidation;
using InTouch.UserService.Core;
using InTouch.UserService.Domain;
using MediatR;

namespace InTouch.Application;

public sealed class CreateRoleCommandHandler(
    IValidator<CreateRoleCommand> validator,
    IEventStoreRepository eventStoreRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    CancellationToken cancellationToken = default
): IRequestHandler<CreateRoleCommand, Result<CreatedResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task<Result<CreatedResponse>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        //Валидация request.
        var _validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!_validationResult.IsValid)
        {
            //возвращаем result с ошибкой валидации.
            return Result<CreatedResponse>.Invalid(_validationResult.AsErrors());
        }

        var _role = RoleFactory.Create(request.Title);
        var eventStrore = new EventStore(
            _role.Id,
            "CreateRoleEntity",
            _role.ToJson());
        try
        {
            await _unitOfWork.GetRepository<Role, Guid>()
                .CreateAsync(_role, cancellationToken);
            await _unitOfWork.GetRepository<EventStore, Guid>().StoreAsync(eventStrore, default);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackChangesAsync(cancellationToken);
            return Result<CreatedResponse>.Error("Ошибка в сохранении данных на сервер!!! " + e.Message);
        }
        return Result<CreatedResponse>.Success(
            new CreatedResponse(_role.Id), "Роль успешно создана!");
    }
}