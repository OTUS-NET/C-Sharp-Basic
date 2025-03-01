### Цель
    
Расширение функционала приложения, разработанного в предыдущем домашнем задании:

- Добавление интерфейсов и классов аналогичных Telegram API, чтобы в будущем было легче переключиться на реального Telegram бота
- Работа с классами и интерфейсами
- Добавление новых комманд

---

### Описание

Перед выполнением нужно ознакомится с [Правила отправки домашнего задания на проверку](https://github.com/OTUS-NET/C-Sharp-Basic/blob/main/Homeworks/README.md)

1.  Подключение библиотеки `Otus.ToDoList.ConsoleBot`
    - Добавить к себе в решение и в зависимости к своему проекту с ботом проект `Otus.ToDoList.ConsoleBot` [GitHub](https://github.com/OTUS-NET/C-Sharp-Basic/tree/main/Homeworks/5%20%D0%9E%D0%9E%D0%9F%20%D0%BA%D0%BB%D0%B0%D1%81%D1%81%D1%8B%20%D0%B8%20%D0%B8%D0%BD%D1%82%D0%B5%D1%80%D1%84%D0%B5%D0%B9%D1%81%D1%8B/Otus.ToDoList.ConsoleBot) 
    - Ознакомиться с классами в папке Types и с README.md
    - Создать класс `UpdateHandler`, который реализует интерфейс `IUpdateHandler`, и перенести в метод `HandleUpdateAsync` обработку всех команд. Вместо Console.WriteLine использовать `SendMessage` у `ITelegramBotClient`
    - Перенести try/catch в `HandleUpdateAsync`. В Main оставить catch(Exception)
2. Удалить команду `/echo`
3. Изменение логики команды `/start`
    - Не нужно запрашивать имя
    - Добавить класс `User`
        - Свойства
            - Guid UserId
            - long TelegramUserId
            - string TelegramUserName
            - DateTime RegisteredAt
        - Конструктор `User(long telegramUserId, string telegramUserName)`
    - Подумать над наличием get set и над их модификаторами доступа. Нужно чтобы извне класса изменить значения было нельзя
4.  Добавление класса сервиса `UserService`
    - Добавить интерфейс `IUserService`
    ```csharp
    interface IUserService
    {
        User RegisterUser(long telegramUserId, string telegramUserName);
        User? GetUser(long telegramUserId);
    }
    ```
    - Создать класс `UserService`, который реализует интерфейс `IUserService`
    - Добавить использование `IUserService` в `UpdateHandler`. Получать `IUserService` нужно через конструктор
    - При команде `/start` нужно вызвать метод `IUserService.RegisterUser`.
    - Если пользователь не зарегистрирован, то ему доступны только команды `/help` `/info`
5.  Добавление класса `ToDoItem`
    - Добавить enum `ToDoItemState` с двумя значениями
        - Active
        - Completed
    - Добавить класс `ToDoItem`
        - Свойства
            - Guid Id
            - User User
            - string Name
            - DateTime CreatedAt
            - ToDoItemState State
            - DateTime? StateChangedAt
        - Методы
            - конструктор `ToDoItem(User user, string name)`
            - `void MarkAsCompleted()` - изменяет State на Completed и обновляет StateChangedAt
    - Подумать над наличием get set и над их модификаторами доступа. Нужно чтобы извне класса изменить значения было нельзя
    - Добавить использование класса `ToDoItem` вместо хранения только имени задачи
6.  Изменение логики `/showtasks`
    - Выводить только задачи с `ToDoItemState.Active`
    - Добавить вывод CreatedAt. Пример: 1. Имя задачи - 01.01.2025 00:00:00
7.  Добавление класса сервиса `ToDoService`
    - Добавить интерфейс `IToDoService`
    ```csharp
    public interface IToDoService
    {
        //Возвращает ToDoItem для UserId со статусом Active
        IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId);
        ToDoItem Add(User user, string name);
        void MarkAsCompleted(ToDoItem item);
        void Delete(ToDoItem item);
    }
    ```
    - Создать класс `ToDoService`, который реализует интерфейс `IToDoService`. Перенести в него логику обработки команд
    - Добавить использование `IToDoService` в `UpdateHandler`. Получать `IToDoService` нужно через конструктор
8.  Добавление команды `/completetask`
    - Добавить обработку новой команды `/completetask`. При обработке команды использовать метод `IToDoService.MarkAsCompleted`
9.  Добавление команды `/showalltasks`
    - Добавить обработку новой команды `/showalltasks`. По ней выводить команды с любым `State` и добавить `State` в вывод
    - Пример: 1. (Active) Имя задачи - 01.01.2025 00:00:00
10.  Обновить `/help`

---

### Критерии оценивания

- Пункты 1-8 - 9 баллов
- Пункты 9-10 - 1 балл

Для зачёта домашнего задания достаточно 9 баллов.