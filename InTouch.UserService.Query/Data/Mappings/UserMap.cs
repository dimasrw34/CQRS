using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace InTouch.UserService.Query;

public class UserMap : IReadDbMapping
{
    public void Configure()
    {
        // TryRegisterClassMap: Регистрируем класс map если он еще не был зарегистрирован.
        BsonClassMap.TryRegisterClassMap<UserQueryModel>(classMap =>
        {
            classMap.AutoMap();
            classMap.SetIgnoreExtraElements(true);
            
            
            classMap.MapMember(user => user.Id)
                .SetIsRequired(false);
            
            classMap.MapMember(user => user.Login)
                .SetIsRequired(false);
            
            classMap.MapMember(user => user.Password)
                .SetIsRequired(false);
            
            classMap.MapMember(user => user.Name)
                .SetIsRequired(false);
            
            classMap.MapMember(user => user.LastName)
                .SetIsRequired(false);
            
            classMap.MapMember(user => user.Email)
                .SetIsRequired(false);
            
            classMap.MapMember(user => user.Phone)
                .SetIsRequired(false);
        });
    }
}