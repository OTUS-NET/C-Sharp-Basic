#pragma warning disable CS8618

namespace Otus.ToDoList.ConsoleBot.Types;
/// <summary>
/// Сущность - сообщение из чата с ботом
/// </summary>
public class Message
{
    /// <summary>
    /// Идетнификатор сообщения
    /// </summary>
    public int Id { get; init; }
    /// <summary>
    /// Текст сообщения
    /// </summary>
    public string Text { get; init; }
    /// <summary>
    /// Чат которому принадлежит сообщение
    /// </summary>
    public Chat Chat { get; init; }
    /// <summary>
    /// Пользователь , отправивший сообщение
    /// </summary>
    public User From { get; init; }
}
