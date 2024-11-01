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
    IDbContext dbContext,
    IUserWriteOnlyRepository<User,Guid> userWriteOnlyRepository,
    IEventStoreRepository eventStoreRepository,
    IMediator mediator
    ) : IRequestHandler<CreateUserCommand, Result<CreatedUserResponse>>
{
    private readonly IDbContext _context = dbContext;
    private readonly IUserWriteOnlyRepository<User, Guid> _userWriteOnlyRepository = userWriteOnlyRepository;
   
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
        if (await userWriteOnlyRepository.ExistByEmailAsync(email))
        {
            return Result<CreatedUserResponse>.Error("Пользователь с данной электронной почтой уже существует.");
        }

        // Создание экземпляра сущности пользователя.
        // При создании экземпляра будет создано событие «UserCreatedEvents».
        var _user = UserFactory.Create(
            email,
            request.Password,
            request.Name,
            request.Surname,
            request.Phone);
        
        //Создаем ventStore
       var eventStore = new EventStore(
           _user.Id,
           "CreateUserEntity",
           _user.ToJson());
       
        // Сохранение изменений в БД и срабатывание событий.
        try
        {
            userWriteOnlyRepository.AddAsync(_user);
            eventStoreRepository.StoreAsync(eventStore);
            dbContext.CommitAsync();
        }
        catch (Exception e)
        {
            await dbContext.RollbackAsync();
            return Result<CreatedUserResponse>.Error("Ошибка в сохранении данных на сервер!!! " + e.Message);
        }

        //уведомляем через MediatR.INotify для сохранения в БД событий
        foreach (var @event in _user.DomainEvents)
        {
            await mediator.Publish(@event, cancellationToken);
        }
        
        // Возвращаем ИД нового пользователя и сообщение об успехе.
        return Result<CreatedUserResponse>.Success(
            new CreatedUserResponse(_user.Id), "Пользователь успешно зарегистрирован!");
    }
}