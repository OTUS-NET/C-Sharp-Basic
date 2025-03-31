### Цель
    
Расширение функционала приложения, разработанного в предыдущих домашних заданиях:

- Работа с классами и интерфейсами. Добавление репозиториев
- Добавление новых команд
- Работа с лямбдами и кортежами

---

### Описание

Перед выполнением нужно ознакомится с [Правила отправки домашнего задания на проверку](https://github.com/OTUS-NET/C-Sharp-Basic/blob/main/Homeworks/README.md)

1. Добавление репозитория `IUserRepository`
    - Добавить интерфейс `IUserRepository`
    ```csharp
    interface IUserRepository
    {
        User? GetUser(Guid userId);
        User? GetUserByTelegramUserId(long telegramUserId);
        void Add(ToDoUser user);
    }
    ```
    - Создать класс `InMemoryUserRepository`, который реализует интерфейс `IUserRepository`. В качестве хранилища использовать List
    - Добавить использование `IUserRepository` в `UserService`. Получать `IUserRepository` нужно через конструктор
2. Добавление репозитория `IToDoRepository`
    - Добавить интерфейс `IToDoRepository`
    ```csharp
    interface IToDoRepository
    {
        //Возвращает ToDoItem для UserId со статусом Active
        IReadOnlyList<ToDoItem> GetActiveByUserId(Guid userId);
        void Add(ToDoItem item);
        void Update(ToDoItem item);
        void Delete(Guid id);
        //Проверяет есть ли задача с таким именем у пользователя
        bool ExistsByName(Guid userId, string name);
        //Возвращает количество активных задач у пользователя
        int CountActive(Guid userId); 
    }
    ```
    - Создать класс `InMemoryToDoRepository`, который реализует интерфейс `IToDoRepository`. В качестве хранилища использовать List
    - Добавить использование `IToDoRepository` в `ToDoService`. Получать `IToDoRepository` нужно через конструктор
3. Кортежи. Добавление команды `/report`
    - Добавить метод `IReadOnlyList<ToDoItem> GetAllByUserId(Guid userId);` в интерфейс `IToDoRepository`. Метод должен возвращать все задачи пользователя
    - Добавить интерфейс `IToDoReportService`
    ```csharp
    interface IToDoReportService
    {
        (int total, int completed, int active, DateTime generatedAt) GetUserStats(Guid userId);
    }
    ```
    - Создать класс `ToDoReportService`, который реализует интерфейс `IToDoReportService`.
    - Добавить обработку новой команды `/report`. Нужно использовать `IToDoReportService`
    - Пример вывода: Статистика по задачам на 01.01.2025 00:00:00. Всего: 10; Завершенных: 7; Активных: 3;
4. Лямбды. Добавление команды `/find`
    - Добавить метод `IReadOnlyList<ToDoItem> Find(Guid userId, Func<ToDoItem, bool> predicate);` в интерфейс `IToDoRepository`. Метод должен возвращать все задачи пользователя, которые удовлетворяют предикату.
    - Добавить метод `IReadOnlyList<ToDoItem> Find(ToDoUser user, string namePrefix);` в интерфейс `IToDoService`. Метод должен возвращать все задачи пользователя, которые начинаются на namePrefix. Для этого нужно использовать метод `IToDoRepository.Find`
    - Добавить обработку новой команды `/find`.
    - Пример команды: `/find Имя`
    - Вывод в консоль должен быть как в `/showtask`
5. Рекомендуемая структура проекта
    ```
    Project/
    ├── Core/
    │   ├── DataAccess/
    │   │   ├── IUserRepository.cs
    │   │   ├── IToDoRepository.cs
    │   │   └── ...
    │   ├── Entities/
    │   │   ├── ToDoUser.cs
    │   │   ├── ToDoItem.cs
    │   │   └── ...
    │   ├── Exceptions/
    │   │   ├── TaskCountLimitException.cs
    │   │   ├── TaskLengthLimitException.cs
    │   │   └── ...
    │   └── Services/
    │       ├── IUserService.cs
    │       ├── UserService.cs
    │       └── ...
    │
    ├── Infrastructure/
    │   └── DataAccess/
    │       ├── InMemoryUserRepository.cs
    │       ├── InMemoryToDoRepository.cs
    │       └── ...
    │
    └── TelegramBot/
        ├── UpdateHandler.cs
        └── ...
    ```
6. Обновить `/help`
---

### Критерии оценивания

- Пункты 1-2 - 5 баллов
- Пункт 3 - 2 балла
- Пункт 4 - 2 балла
- Пункт 5 - 1 балл

Для зачёта домашнего задания достаточно 8 баллов.