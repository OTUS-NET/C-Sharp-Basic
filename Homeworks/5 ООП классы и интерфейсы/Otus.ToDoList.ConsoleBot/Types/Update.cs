#pragma warning disable CS8618

namespace Otus.ToDoList.ConsoleBot.Types;
/// <summary>
/// Обновление, поступающее от бота
/// </summary>
public class Update
{
    /// <summary>
    /// Сообщение из чата с ботом
    /// </summary>
    public Message Message { get; init; }
}
