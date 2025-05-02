using Otus.ToDoList.ConsoleBot.Types;

namespace Otus.ToDoList.ConsoleBot;
/// <summary>
///  Интерфейс обработчика обновлений для клиента, работающего с ботом
/// </summary>
public interface IUpdateHandler
{
    /// <summary>
    /// Метод-обработчик обновлений для клиента работающего с ботом
    /// </summary>
    /// <param name="botClient">Клиент работающий с ботом</param>
    /// <param name="update"> Полученное обновлене от бота</param>
    void HandleUpdateAsync(ITelegramBotClient botClient, Update update);
}
