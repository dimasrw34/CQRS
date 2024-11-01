using InTouch.UserService.Core;

namespace InTouch.UserService.Domain;

public sealed class User : BaseEntity, IAggregateRoot
{
    private bool _isDeleted;

    #region Базовый конструктор
    /// <summary>
    /// Базовый конструктор для ORM
    /// </summary>
    public User() { }
    
    #endregion Базовый конструктор

    #region Конструктор и методы с событиями
    /// <summary>
    /// Инициализирует новый экземпляр класса User.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <param name="name"></param>
    /// <param name="surname"></param>
    /// <param name="phone"></param>
    public User(Email email, string password, string name, string surname,string phone)
        {
            Email = email;
            Password = password;
            Name = name;
            Surname = surname;
            Phone = phone;
            //Добавляем событие в брокер событий
            AddDomainEvent(new UserCreatedEvent(Id, email.Address, password,name,surname, phone));
            
        }
    /// <summary>
    /// Меняет почтовый адрес пользователя
    /// </summary>
    /// <param name="newEmail">Новый почтовый адрес</param>
    public void ChangeEmail(Email newEmail)
    {
        if(Email.Equals(newEmail))
            return;
        Email = newEmail;
        AddDomainEvent(new UserUpdatedEvent(Id, newEmail.Address, Password, Name, Surname, Phone));
    }
    
    
    
    /// <summary>
    /// Удаляет юзера
    /// </summary>
    public void Delete()
    {
        if (_isDeleted) return;
        _isDeleted = true;
        AddDomainEvent(new UserDeletedEvent(Id, Email.Address, Password, Name, Surname, Phone));
    }

    #endregion Конструктор и методы с событиями
  
    #region Свойства
    
    
    /// <summary>
    /// Получаем почту пользователя
    /// </summary>
    public Email Email { get; private set; }
    
    /// <summary>
    /// Получаем пароль пользователя
    /// </summary>
    public string Password { get; }

    /// <summary>
    /// Получаем имя пользователя
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Получаем фамилию пользователя
    /// </summary>
    public string Surname { get; }
    
    /// <summary>
    /// Получаем телефон пользователя
    /// </summary>
    public string Phone { get; }
    #endregion Свойства
}