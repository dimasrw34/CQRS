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
    ) : IRequestHandler<CreateUserCommand, Result<CreatedUserResponse>>
{
    private readonly IUserWriteOnlyRepository<User, Guid> _userWriteOnlyRepository = userWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
   
    public async Task<Result<CreatedUserResponse>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        //Валидация request.
        var _validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!_validationResult.IsValid)
        {
            //возвращаем result с ошибкой валидации.
            return Result<CreatedUserResponse>.Invalid(_validationResult.AsErrors());
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
       
        // Сохранение изменений в БД и срабатывание событий.
        var a =  await userWriteOnlyRepository.CreateAsync(_user,cancellationToken);
        await eventStoreRepository.StoreAsync(eventStore);
        //await _unitOfWork.GetRepository<User,Guid>().CreateAsync(_user,cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        /*try
        {
            var a =  await userWriteOnlyRepository.CreateAsync(_user,cancellationToken);
            await eventStoreRepository.StoreAsync(eventStore);
            //await _unitOfWork.GetRepository<User,Guid>().CreateAsync(_user,cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            return Result<CreatedUserResponse>.Error("Ошибка в сохранении данных на сервер!!! " + e.Message);
        }
        */

        //уведомляем через MediatR.INotify для сохранения в БД событий
        /*foreach (var @event in _user.DomainEvents)
        {
            await mediator.Publish(@event, cancellationToken);
        }*/
        
        // Возвращаем ИД нового пользователя и сообщение об успехе.
        return Result<CreatedUserResponse>.Success(
            new CreatedUserResponse(_user.Id), "Пользователь успешно зарегистрирован!");
    }
}