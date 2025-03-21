using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using MediatR;

using InTouch.UserService.Domain;
using InTouch.Infrastructure.Data;
using InTouch.UserService.Core;

namespace InTouch.Application;

public sealed class CreateUserCommandHandler(
    IValidator<CreateUserCommand> validator,
    IUserWriteOnlyRepository<User,Guid> userWriteOnlyRepository,
    IEventStoreRepository eventStoreRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    CancellationToken cancellationToken = default
    ) : IRequestHandler<CreateUserCommand, Result<CreatedResponse>>
{
    private readonly IUserWriteOnlyRepository<User, Guid> _userWriteOnlyRepository = userWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
   
    public async Task<Result<CreatedResponse>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        //Валидация request.
        var _validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!_validationResult.IsValid)
        {
            //возвращаем result с ошибкой валидации.
            return Result<CreatedResponse>.Invalid(_validationResult.AsErrors());
        }
        
        // Создаем email value object.
        var email = Email.Create(request.Email).Value;
        
        // Проверяем, что пользователь с такой почтой создан. 
        /*if (await userWriteOnlyRepository.ExistByEmailAsync(email))
        {
            return Result<CreatedUserResponse>.Error("Пользователь с данной электронной почтой уже существует.");
        }*/

        // Создание экземпляра сущности пользователя.
        // При создании экземпляра будет создано событие «UserCreatedEvents».
        var _user = UserFactory.Create(
            request.Login,
            request.Password,
            request.FirstName,
            request.LastName,
                email,
            request.Phone);
        
        //Создаем ventStore
       var eventStore = new EventStore(
           _user.Id,
           "CreateUserEntity",
           _user.ToJson());

       try
        {
            //где бы не произошла ошибка, данные всегда будут консистентны
            
            // Сохранение изменений в БД и срабатывание событий.
            await _unitOfWork.GetRepository<User, Guid>().CreateAsync(_user, cancellationToken);
            await _unitOfWork.GetRepository<EventStore, Guid>().StoreAsync(eventStore, default);
            
            //уведомляем через MediatR.INotify для сохранения в БД событий
            /*foreach (var @event in _user.DomainEvents)
                {
                    await mediator.Publish(@event, cancellationToken);
                }*/
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackChangesAsync(cancellationToken);
            return Result<CreatedResponse>.Error("Ошибка в сохранении данных на сервер!!! " + e.Message);
        }
        // Возвращаем ИД нового пользователя и сообщение об успехе.
        return Result<CreatedResponse>.Success(
            new CreatedResponse(_user.Id), "Пользователь успешно зарегистрирован!");
    }
}