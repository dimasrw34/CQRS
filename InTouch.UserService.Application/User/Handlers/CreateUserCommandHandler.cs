using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using MediatR;

using InTouch.UserService.Domain;
using InTouch.UserService.Core;
using InTouch.UserService.Infrastructure.Data;


namespace InTouch.UserService.Application;

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

    public record Person(int Age, int Height);
    public string CategorizePerson (Person person) => person switch
    {
        var (age, height) when age < 18  && height < 160 => "Young and short",
        var (age, height) when age < 18  && height >= 160 => "Young and tall",
        _ => "Unknown person",
    };

   
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
        
        
        
        // Проверяем, что пользователь с таким именем создан. 
        
        if (await _unitOfWork.Users.ExistByLoginAsync(new FindByIdAndLoginSpecification (
            new FindByIdSpecification(_user.Id),
            new FindByLoginSpecification (_user.Login))))
        {
            return Result<CreatedResponse>.Error("Пользователь с таким именем уже существует.");
        }
        
        //Создаем ventStore
       var eventStore = new EventStore(
           _user.Id,
           "CreateUserEntity",
           _user.ToJson());

       try
        {
            //где бы не произошла ошибка, данные всегда будут консистентны
            
            // Сохранение изменений в БД и срабатывание событий.
            //await _unitOfWork..GetRepository<User, Guid>().CreateAsync(_user, cancellationToken);
            //await _unitOfWork.GetRepository<EventStore, Guid>().StoreAsync(eventStore, default);
            
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