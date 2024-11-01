using AutoMapper;
using InTouch.UserService.Domain;

namespace InTouch.UserService.Query;

public class EventToQueryModelProfile : Profile
{
    public EventToQueryModelProfile()
    {
        CreateMap<UserCreatedEvent, UserQueryModel>(MemberList.Destination)
            .ConstructUsing(@event => CreateUserQueryModel(@event));
        
        CreateMap<UserUpdatedEvent, UserQueryModel>(MemberList.Destination)
            .ConstructUsing(@event => CreateUserQueryModel(@event));
        
        CreateMap<UserDeletedEvent, UserQueryModel>(MemberList.Destination)
            .ConstructUsing(@event => CreateUserQueryModel(@event));
    }

    public override string ProfileName => nameof(EventToQueryModelProfile);

    private static UserQueryModel CreateUserQueryModel<TEvent>(TEvent @event) where TEvent : UserBaseEvent =>
        new(@event.Id, @event.Email, @event.Password, @event.Name, @event.Surname, @event.Phone);
}