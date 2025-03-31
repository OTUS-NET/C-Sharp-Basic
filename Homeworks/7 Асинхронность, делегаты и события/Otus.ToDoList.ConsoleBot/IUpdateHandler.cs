using Otus.ToDoList.ConsoleBot.Types;

namespace Otus.ToDoList.ConsoleBot;
public interface IUpdateHandler
{
    Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct);
    Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken ct);
}
