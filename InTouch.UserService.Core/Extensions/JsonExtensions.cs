using System;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Text.Unicode;


namespace InTouch.UserService.Core;

public static class JsonExtensions
{
    private static readonly Lazy<JsonSerializerOptions> LazyOptions =
        new(() => new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin,UnicodeRanges.Cyrillic),
            WriteIndented = true
        }.Configure(), isThreadSafe: true);

    /// <summary>
    /// Преобразует строку JSON в объект типа T.
    /// </summary>
    /// <param name="value">Тип объекта для десериализации.</param>
    /// <typeparam name="T">Строка JSON для десериализации.</typeparam>
    /// <returns>Десериализованный объект типа T.</returns>
    public static T FromJson<T>(this string value) =>
        value != null
            ? JsonSerializer.Deserialize<T>(value, LazyOptions.Value)
            : default;

    /// <summary>
    /// Преобразует объект в строку JSON.
    /// </summary>
    /// <param name="value">Тип объекта.</param>
    /// <typeparam name="T">Объект для преобразования.</typeparam>
    /// <returns>Строковое представление объекта в формате JSON.</returns>
    public static string? ToJson<T>(this T value) =>
        !value.IsDefault()
            ? JsonSerializer.Serialize(value, LazyOptions.Value)
            : default;
    
    /// <summary>
    /// Настраивает экземпляр JsonSerializerOptions.
    /// </summary>
    /// <param name="jsonSettings">Экземпляр JsonSerializerOptions для настройки.</param>
    /// <returns>Настроенный экземпляр JsonSerializerOptions.</returns>
    public static JsonSerializerOptions Configure(this JsonSerializerOptions jsonSettings)
    {
        jsonSettings.WriteIndented = false;
        jsonSettings.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        jsonSettings.ReadCommentHandling = JsonCommentHandling.Skip;
        jsonSettings.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        jsonSettings.TypeInfoResolver = new PrivateConstructorResolver();
        jsonSettings.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        return jsonSettings;
    }
    
    internal sealed class PrivateConstructorResolver : DefaultJsonTypeInfoResolver
    {
        /// <summary>
        /// Получает информацию о типе JSON для указанного типа с поддержкой создания объектов с закрытыми конструкторами. 
        /// </summary>
        /// <param name="type">Тип, для которого необходимо получить информацию о типе JSON.</param>
        /// <param name="options"><see cref="JsonSerializerOptions"/>, используемый для сериализации.</param>
        /// <returns>Информация о типе JSON для указанного типа</returns>
        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            var jsonTypeInfo = base.GetTypeInfo(type, options);
            
            // Проверяем, является ли тип Object, не имеет ли он открытого конструктора и не установлен CreateObject
            if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object
                && jsonTypeInfo.CreateObject is null
                && jsonTypeInfo.Type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length == 0)
            {
                // Установливаем CreateObject на лямбда-выражение, которое создает экземпляр с помощью закрытого конструктора.
                jsonTypeInfo.CreateObject = () => Activator.CreateInstance(jsonTypeInfo.Type, true);
            }

            return jsonTypeInfo;
        }
    }
}