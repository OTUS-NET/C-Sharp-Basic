### Цель
    
Расширение функционала приложения, разработанного в предыдущих домашних заданиях:

- Добавление интерфейсов и классов аналогичных Telegram API, чтобы в будущем было легче переключиться на реального Telegram бота
- Работа с интерфейсами

---

### Описание

Перед выполнением нужно ознакомится с [Правила отправки домашнего задания на проверку](https://github.com/OTUS-NET/C-Sharp-Basic/blob/main/Homeworks/README.md)

1. Подключение библиотеки `Otus.ToDoList.ConsoleBot`
    - Добавить к себе в решение и в зависимости к своему проекту с ботом проект `Otus.ToDoList.ConsoleBot` [GitHub](https://github.com/OTUS-NET/C-Sharp-Basic/tree/main/Homeworks/05.2%20%D0%9E%D0%9E%D0%9F%20%D0%B8%D0%BD%D1%82%D0%B5%D1%80%D1%84%D0%B5%D0%B9%D1%81%D1%8B/Otus.ToDoList.ConsoleBot). ![Добавление проекта](Demo.gif)
    - Ознакомиться с классами в папке Types и с README.md
    - Создать класс `UpdateHandler`, который реализует интерфейс `IUpdateHandler`, и перенести в метод `HandleUpdateAsync` обработку всех команд. Вместо Console.WriteLine использовать `SendMessage` у `ITelegramBotClient`
    - Перенести try/catch в `HandleUpdateAsync`. В Main оставить catch(Exception)
    - Для вывода в конcоль сообщений использовать метод `ITelegramBotClient.SendMessage`
    - Код библиотеки `Otus.ToDoList.ConsoleBot` не нужно изменять
2. Удалить команду `/echo`
3. Изменение класса `ToDoUser`
    - Добавить свойство long TelegramUserId
    - В конструктор добавить аргумент long telegramUserId
4. Добавление класса сервиса `UserService`
    - Добавить интерфейс `IUserService`
    ```csharp
    interface IUserService
    {
        ToDoUser RegisterUser(long telegramUserId, string telegramUserName);
        ToDoUser? GetUser(long telegramUserId);
    }
    ```
    - Создать класс `UserService`, который реализует интерфейс `IUserService`. Заполнять telegramUserId и telegramUserName нужно из значений `Update.Message.From`
5. Изменение логики команды `/start`
    - Не нужно запрашивать имя
    - Добавить использование `IUserService` в `UpdateHandler`. Получать `IUserService` нужно через конструктор
    - Для обработки команды нужно использовать `IUserService.GetUser`. Если пользователь не найден, то вызывать `IUserService.RegisterUser`
    - Если пользователь не зарегистрирован, то ему доступны только команды `/help` `/info`
6. Добавление класса сервиса `ToDoService`
    - Добавить интерфейс `IToDoService`
    ```csharp
    public interface IToDoService
    {
        IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId);
        //Возвращает ToDoItem для UserId со статусом Active
        IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId);
        ToDoItem Add(ToDoUser user, string name);
        void MarkCompleted(Guid id);
        void Delete(Guid id);
    }
    ```
    - Создать класс `ToDoService`, который реализует интерфейс `IToDoService`. Перенести в него логику обработки команд. Проверки на максимальное количество задач, на максимальную длину задачи и на дубликаты тоже нужно перенести в `ToDoService`.
    - Добавить использование `IToDoService` в `UpdateHandler`. Получать `IToDoService` нужно через конструктор
    - Изменить формат обработки команды `/addtask`. Нужно сразу передавать имя задачи в команде. Пример: `/addtask Новая задача`
    - Изменить формат обработки команды `/removetask`. Нужно сразу передавать номер задачи в команде. Пример: `/removetask 2`
7. Изменение команды `/completetask`
    - При обработке команды использовать метод `IToDoService.MarkAsCompleted`

Примечание: Можно заменить catch с разными типами исключений, если в них нет кастомной логики, на один catch(Exception ex). Так как в предыдущем задание сatch с разными типами исключений добавлялись в учебных целям и в реальных проектах не нужно делать catch на каждый тип исключения, если в них нет специальной логики.

---

### Критерии оценивания

- Пункт 1 - 2 балла
- Пункт 2 - 1 балл
- Пункт 3 - 1 балл
- Пункт 4 - 1 балла
- Пункт 5 - 2 балла
- Пункт 6 - 2 балла
- Пункт 7 - 1 балл

Для зачёта домашнего задания достаточно 8 баллов.
