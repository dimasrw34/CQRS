using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using InTouch.Infrastructure.Data;
using InTouch.UserService.Core;
using InTouch.UserService.Domain;
using MediatR;

namespace InTouch.Application;

public class DeleteUserCommandHandler(
    IValidator<DeleteUserCommand> validator,
    IDbContext dbContext,
    IUserWriteOnlyRepository<User, Guid> repository,
    IEventStoreRepository eventStoreRepository,
    IMediator mediator) 
    : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IDbContext _context = dbContext;
    private readonly IUserWriteOnlyRepository<User, Guid> _repository = repository;
    
    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        //валидируккм request
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validatorResult.IsValid)
            return Result.Invalid(validatorResult.AsErrors());
        
        //проверяем наличие юзера в базе
        var user = _repository.GetByIdAsync(request.Id).Result;
        if (user is null)
            return Result.NotFound("Пользователь с идентификатором " + request.Id + " отсутствует.");
        
        //делеаем состояние удален, будет добавлен UserDeletedEvent
        user.Delete();
        
        //создаем хранилище событий для передачи в БД
        var eventStore = new EventStore(
            user.Id,
            "DeleteUserEntity",
            user.ToJson());
        
        try
        {
            //удаляем из репозиторя
            await _repository.DeleteAsync(user.Id);
            //добвляем событие состояния
            await eventStoreRepository.StoreAsync(eventStore);
            //сохраняем
            await _context.CommitAsync();
        }
        catch (Exception e)
        {
            await _context.RollbackAsync();
            return Result.Error("Ошибка сервера: " + e);
        }
        
        //срабатыаем MediatR.INotify
        foreach (var @event in user.DomainEvents)
        {
            await mediator.Publish(@event, cancellationToken);
        }
        
        return Result.SuccessWithMessage("Пользователь удален");
    }
}
