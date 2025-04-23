using Otus.ToDoList.ConsoleBot.Types;

namespace Otus.ToDoList.ConsoleBot;
/// <summary>
/// Консольный клиент для бота
/// </summary>
public class ConsoleBotClient : ITelegramBotClient
{
    private readonly Chat _chat;
    private readonly User _user;

    public ConsoleBotClient()
    {
        _chat = new Chat { Id = Random.Shared.Next() }; // генерируем чат с рандомным Id-шником
        _user = new User { Id = Random.Shared.Next(), Username = $"ConsoleUser_{Guid.NewGuid()}" };  // генерируем пользовпателя который общается с ботом
    }

    public void SendMessage(Chat chat, string text)
    {
        ArgumentNullException.ThrowIfNull(chat, nameof(chat));
        ArgumentNullException.ThrowIfNull(text, nameof(text));
        if (_chat.Id != chat.Id)
            throw new ArgumentException($"Invalid chat.Id. Support {_chat.Id}, but was {chat.Id}");

        WriteLineColor($"Бот: {text}", ConsoleColor.Blue);
    }

    public void StartReceiving(IUpdateHandler handler)
    {
        ArgumentNullException.ThrowIfNull(handler, nameof(handler));

        var cts = new CancellationTokenSource();    // дает возможность при завершении работы прервать запущенный асинхронные операции    
        ConsoleCancelEventHandler cancelHandler = (sender, e) =>  //Обработчик выхода из консоли(по нажатию Ctrl+C)
        {
            cts.Cancel(); // прерываем запущенные синхронные операции
            e.Cancel = true;
        };
        Console.CancelKeyPress += cancelHandler; //Добавляем обработчик выхода из консоли

        try
        {
            WriteLineColor("Бот запущен. Введите сообщение", ConsoleColor.Magenta);
            var counter = 0; // счетчик полученных сообщений, который мы используем, как идентификатор для получаемого сообщения

            while (cts.IsCancellationRequested is false)    // Пока не cts.Cancel(); ( пока не нажали Ctrl+C)
            {
                var input = Console.ReadLine();  // вводим в консоль - считываем сообщение в консольный бот
                if (input is null)
                    break;

                var update = new Update   // обновление - которое генерирует консольный бот
                {
                    Message = new Message  // сообщение - содержимое геннерируемого обновления
                    {
                        Id = Interlocked.Increment(ref counter),
                        Text = input,
                        Chat = _chat,
                        From = _user
                    }
                };

                handler.HandleUpdateAsync(this, update); // обрабатываем сгенерированное сообщение
            }
        }
        finally
        {
            Console.CancelKeyPress -= cancelHandler; // очистка ресурсов, отписываемся от обработчика
            cts.Dispose();    // очистка ресурсов
            WriteLineColor("Бот остановлен", ConsoleColor.Magenta);
        }
    }

    /// <summary>
    /// Вывод в консоль
    /// </summary>
    /// <param name="text">Текст , который нужно вывести</param>
    /// <param name="color">Цвет текста</param>
    private static void WriteLineColor(string text, ConsoleColor color)
    {
        var currentColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = currentColor;
    }
}
