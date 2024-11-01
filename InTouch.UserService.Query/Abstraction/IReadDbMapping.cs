namespace InTouch.UserService.Query;

/// <summary>
/// Представляет интерфейс для чтения сопоставлений базы данных.
/// </summary>
public interface IReadDbMapping
{
    /// <summary>
    /// Настраивает сопоставления для чтения из базы данных.
    /// </summary>
    void Configure();
}