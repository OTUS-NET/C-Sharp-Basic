using Otus.ToDoList.ConsoleBot.Types;

namespace Otus.ToDoList.ConsoleBot;
public interface IUpdateHandler
{
    void HandleUpdateAsync(ITelegramBotClient botClient, Update update);
}
