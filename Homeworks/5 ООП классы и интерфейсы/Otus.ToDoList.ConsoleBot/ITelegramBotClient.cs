using Otus.ToDoList.ConsoleBot.Types;

namespace Otus.ToDoList.ConsoleBot;
/// <summary>
/// Интерфейс клиента для будущего телеграм-бота
/// </summary>
public interface ITelegramBotClient
{
    /// <summary>
    /// Метод начала взаимодействия клиента с телеграм-ботом
    /// </summary>
    /// <param name="handler">Обработчик обновлений поступающих с бота</param>
    void StartReceiving(IUpdateHandler handler);
    /// <summary>
    /// Метод отправки сообщений в чат с телеграм-ботом
    /// </summary>
    /// <param name="chat">Чат, в который отправляем сообщение</param>
    /// <param name="text">Текстовое сообщение для отправки</param>
    void SendMessage(Chat chat, string text);
}
