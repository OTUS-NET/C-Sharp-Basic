using Otus.ToDoList.ConsoleBot.Types;

namespace Otus.ToDoList.ConsoleBot;
public interface ITelegramBotClient
{
    void StartReceiving(IUpdateHandler handler, CancellationToken ct);
    Task SendMessage(Chat chat, string text, CancellationToken ct);
}
