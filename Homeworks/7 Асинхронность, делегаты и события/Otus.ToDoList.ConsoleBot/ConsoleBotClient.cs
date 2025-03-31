using Otus.ToDoList.ConsoleBot.Types;

namespace Otus.ToDoList.ConsoleBot;
public class ConsoleBotClient : ITelegramBotClient
{
    private readonly Chat _chat;
    private readonly User _user;

    public ConsoleBotClient()
    {
        _chat = new Chat { Id = Random.Shared.Next() };
        _user = new User { Id = Random.Shared.Next(), Username = $"ConsoleUser_{Guid.NewGuid()}" };
    }

    public async Task SendMessage(Chat chat, string text, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(chat, nameof(chat));
        ArgumentNullException.ThrowIfNull(text, nameof(text));
        if (_chat.Id != chat.Id)
            throw new ArgumentException($"Invalid chat.Id. Support {_chat.Id}, but was {chat.Id}");

        await WriteLineColorAsync($"Бот: {text}", ConsoleColor.Blue, ct);
    }

    public void StartReceiving(IUpdateHandler handler, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(handler, nameof(handler));

        try
        {
            WriteLineColor("Бот запущен. Введите сообщение", ConsoleColor.Magenta);
            var counter = 0;

            while (ct.IsCancellationRequested is false)
            {
                var input = Console.ReadLine();
                if (input is null)
                    break;

                var update = new Update
                {
                    Message = new Message
                    {
                        Id = Interlocked.Increment(ref counter),
                        Text = input,
                        Chat = _chat,
                        From = _user
                    }
                };

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await handler.HandleUpdateAsync(this, update, ct);
                    }
                    catch (OperationCanceledException) { }
                    catch (Exception ex)
                    {
                        try
                        {
                            await handler.HandleErrorAsync(this, ex, ct);
                        }
                        catch (OperationCanceledException) { }
                    }
                }, ct);
            }
        }
        finally
        {
            WriteLineColor("Бот остановлен", ConsoleColor.Magenta);
        }
    }

    private static void WriteLineColor(string text, ConsoleColor color)
    {
        var currentColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = currentColor;
    }

    private static async Task WriteLineColorAsync(string text, ConsoleColor color, CancellationToken ct)
    {
        var currentColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        await Console.Out.WriteLineAsync(text.AsMemory(), ct);
        Console.ForegroundColor = currentColor;
    }
}
