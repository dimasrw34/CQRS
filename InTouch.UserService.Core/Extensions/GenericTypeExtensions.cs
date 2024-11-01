using System.Linq;

namespace InTouch.UserService.Core;

public static class GenericTypeExtensions
{
    public static bool IsDefault<T>(this T value) =>
        Equals(value, default(T));

    /// <summary>
    /// Возвращает имя универсального типа объекта.
    /// </summary>
    /// <param name="object">Объект, из которого необходимо получить имя универсального типа.</param>
    /// <returns>Имя универсального типа.</returns>
    public static string GetGenericTypeName(this object @object)
    {
        var type = @object.GetType();

        // Провеем, является ли тип дженериком
        if (!type.IsGenericType)
        {
            return type.Name;
        }
        
        // Получите имена общих аргументов и соедините их запятыми
        var genericTypes = string.Join(",", type.GetGenericArguments()
            .Select(t => t.Name).ToArray());

        // Удалить обратную кавычку и добавить общие аргументы к имени типа
        return $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
    }
}