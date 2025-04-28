### Цель
    
Расширение функционала приложения, разработанного в предыдущих домашних заданиях:

- Создать своего Telegram бота
- Работа с Telegram Bot API
- Работа с Reply кнопками

---

### Описание

Перед выполнением нужно ознакомится с [Правила отправки домашнего задания на проверку](https://github.com/OTUS-NET/C-Sharp-Basic/blob/main/Homeworks/README.md)

1. Создайте Telegram-бота через BotFather (https://core.telegram.org/bots/features#botfather)
2. Замена `Otus.ToDoList.ConsoleBot` на библиотеку `Telegram.Bot` (https://github.com/TelegramBots/Telegram.Bot)
    - Удалить `Otus.ToDoList.ConsoleBot`
    - Добавить nuget пакет `Telegram.Bot` (https://github.com/TelegramBots/Telegram.Bot) 
    - Убедиться, что интерфейсы `IUpdateHandler` и `ITelegramBotClient` из `Otus.ToDoList.ConsoleBot` совместимы с аналогичными интерфейсами из `Telegram.Bot`. Поправить using`и в файлах
    - Заменить `ConsoleBotClient` на `TelegramBotClient`
    - Пример кода:
    ```csharp
    var botClient = new TelegramBotClient("<token>");
    var receiverOptions = new ReceiverOptions
    {
        AllowedUpdates = [UpdateType.Message],
        DropPendingUpdates = true
    };
    var handler = new UpdateHandler();
    botClient.StartReceiving(handler, receiverOptions);

    var me = await botClient.GetMe();
    Console.WriteLine($"{me.FirstName} запущен!");

    await Task.Delay(-1); // Устанавливаем бесконечную задержку
    ```
3. Отмена асинхронных операции и остановка приложения при нажатии клавиши A.
    - После запуска Telegram-бота выводите текст "Нажмите клавишу A для выхода" в консоль и ожидайте нажатия любой клавиши.
    - Если нажата клавиша "A" - выходите из программы и отмените все асинхронные операции. В противном случае выводите информацию о Telegram-боте. Информацию нужно взять из метода `botClient.GetMe()`
    - Реализовать отмену асинхронной операции нужно с использованием `CancellationTokenSource`.
4. Добавить `Reply` кнопки с командами
    - До регистрации должна быть доступна только одна кнопка c командой `/start`
    - После регистрации должны быть доступны кнопки c командами `/showalltasks` `/showtasks` `/report`
    - Кнопки создаются через класс `ReplyKeyboardMarkup`
    - При выводе команд `/showalltasks` и `/showtasks` обернуть Id задачи в символы `, чтобы их было удобно копировать
5. Добавить описание команд в нативную кнопку `Menu`
    - Это нужно сделать через метод `ITelegramBotClient.SetMyCommands`

Советы:

1. Избегайте утечку токена вашего Telegram-бота. При отправки ДЗ на проверку убедитесь, что в коде нет токена. Также не делает коммиты в git-репозиторий с токеном.

2. Полезные ресурсы 
    - Документация библиотеки: https://github.com/TelegramBots/Telegram.Bot
    - Пример реализации: https://github.com/TelegramBots/Telegram.Bot.Examples

---

### Критерии оценивания

- Пункты 1-2 - 6 баллов
- Пункт 3 - 2 балла
- Пункт 4 - 1 балла
- Пункт 5 - 1 балла

Для зачёта домашнего задания достаточно 8 баллов.