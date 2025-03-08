using System.Collections.Generic;
using System.Linq;

namespace InTouch.Infrastructure.Data;

public static class UserScripts
{
    /// <summary>
    /// Метод возвращает текст запроса
    /// </summary>
    /// <param name="NameOfMethod">Название метода для которого нужно вернуть запрос</param>
    /// <returns>Текст запроса</returns>
    public static string GetScript(string NameOfMethod) =>
        dictionary.FirstOrDefault(x => x.Key == NameOfMethod).Value;

    /// <summary>
    /// Заносим в словарь по ключу название метода по значению текст запроса
    /// </summary>
    private static void Build()
    {
        dictionary
            .Add("AddAsync", @"BEGIN; INSERT INTO public.users (id, email, password, name, surname, phone) 
                                        VALUES (@userid,  @email, @password,@name, @surname, @phone);");
        dictionary
            .Add("UpdateAsync",@"BEGIN; UPDATE users SET email=@email WHERE id =@id;");
        
        dictionary
            .Add("GetByIdAsync", @"SELECT id, email, password, name, surname, phone " +
                                 "FROM users WHERE id=@id;");
        dictionary
            .Add("ExistByEmailAsync", @"SELECT public.check_user_login (@email);");
        
        dictionary
            .Add("ExistByEmailAndIdAsync", "SELECT public.check_user_login_id (@email, @id);");
        
        dictionary
            .Add("DeleteAsync", "BEGIN; DELETE FROM users WHERE id=@id");
    }
    private static Dictionary<string, string> dictionary;
    static UserScripts()
    {
        Build();    
    }
}