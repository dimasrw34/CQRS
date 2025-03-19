using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using FluentValidation;
using Ardalis.Result.FluentValidation;
using InTouch.UserService.Core;
using InTouch.UserService.Domain;
using MediatR;

namespace InTouch.Application.Permission.Handlers;

public sealed class CreatePermissionCommandHandler(
    IValidator<CreatePermissionCommand> validator,
    IEventStoreRepository eventStoreRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    CancellationToken cancellationToken = default
    ): IRequestHandler<CreatePermissionCommand, Result<CreatedResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task<Result<CreatedResponse>> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
    {
        //Валидация request.
        var _validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!_validationResult.IsValid)
        {
            //возвращаем result с ошибкой валидации.
            return Result<CreatedResponse>.Invalid(_validationResult.AsErrors());
        }

        var _permission = PermissionFactory.Create(request.Title);
        var eventStrore = new EventStore(
            _permission.Id,
            "CreatePermissionEntity",
            _permission.ToJson());
        try
        {
            await _unitOfWork.GetRepository<UserService.Domain.Permission, Guid>()
                .CreateAsync(_permission, cancellationToken);
            await _unitOfWork.GetRepository<EventStore, Guid>().StoreAsync(eventStrore, default);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackChangesAsync(cancellationToken);
            return Result<CreatedResponse>.Error("Ошибка в сохранении данных на сервер!!! " + e.Message);
        }
        return Result<CreatedResponse>.Success(
            new CreatedResponse(_permission.Id), "Пользователь успешно зарегистрирован!");
    }
}