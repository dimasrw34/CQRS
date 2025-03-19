using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using MediatR;

using InTouch.Infrastructure.Data;
using InTouch.UserService.Core;
using InTouch.UserService.Domain;

namespace InTouch.Application;

public class UpdateUserCommandHandler(
    IValidator<UpdateUserCommand> validator,
    IUnitOfWork unitOfWork,
    IUserWriteOnlyRepository<User?, Guid> repository,
    IEventStoreRepository eventStoreRepository,
    IMediator mediator)
    : IRequestHandler<UpdateUserCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserWriteOnlyRepository<User?, Guid> _repository = repository;

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        // Валидируем request
        var _validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!_validationResult.IsValid)
        {
            return Result.Invalid(_validationResult.AsErrors());
        }
        
        // Проверяем, что пользователь с таким именем существует
        var _user = await _repository.GetByIdAsync(request.Id);
        if (_user is null)
        {
            return Result.NotFound($"Пользователь с идентификатором {request.Id}, не существует.");
        }
        
        //Создаем ValueObject Email, валидация в самом объекте Email
        var emailResult = Email.Create(request.Email);
        if (!emailResult.IsSuccess)
            return Result.Error(new ErrorList(emailResult.Errors.ToArray()));
        
        
        //проверяем что пользователь с такой почтой существует
        if (await _repository.ExistByEmailAndIdAsync(emailResult.Value, _user.Id))
            return Result.Error("Почтовый адрес уже  закреплен за данным пользователем");

        //в данный момент запишется событие в доменной модели об изменении почты пользователя
        _user.ChangeEmail(emailResult.Value);
        
        //создаем хранилище событий для передачи в БД
        var eventStore = new EventStore(
            _user.Id,
            "UpdateUserEntity",
            _user.ToJson());
        
        // Сохранение изменений в БД и срабатывание событий.
        try
        {
            await repository.UpdateAsync(_user);
            await eventStoreRepository.StoreAsync(eventStore);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            //await dbContext.RollbackAsync();
            return Result.Error("Ошибка в сохранении данных на сервер!!! " + e.Message);
        }
        
        //срабатыаем MediatR.INotify
        foreach (var @event in _user.DomainEvents)
        {
            await mediator.Publish(@event, cancellationToken);
        }

        return Result.SuccessWithMessage("Почта для пользователя обновлена.");
    }
}