Пример использования:
```csharp
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;

using var cts = new CancellationTokenSource();
var handler = new UpdateHandler();
var botClient = new ConsoleBotClient();
botClient.StartReceiving(handler, cts.Token);

class UpdateHandler : IUpdateHandler
{
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken ct)
    {
        await botClient.SendMessage(update.Message.Chat, $"Получил '{update.Message.Text}'", ct);
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken ct)
    {
        Console.WriteLine($"HandleError: {exception})");
        return Task.CompletedTask;
    }
}
```