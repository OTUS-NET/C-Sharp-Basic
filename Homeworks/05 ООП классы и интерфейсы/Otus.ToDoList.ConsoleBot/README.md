Пример использования:
```csharp
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;

var handler = new UpdateHandler();
var botClient = new ConsoleBotClient();
botClient.StartReceiving(handler);

class UpdateHandler : IUpdateHandler
{
    public void HandleUpdateAsync(ITelegramBotClient botClient, Update update)
    {
        botClient.SendMessage(update.Message.Chat, $"Получил '{update.Message.Text}'");
    }
}
```