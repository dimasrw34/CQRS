namespace InTouch.UserService.Core;

/// <summary>
/// Представляет интерфейс для параметров приложения.
/// </summary>
public interface IAppOptions
{
    static abstract string ConfigSectionPath { get; }
}