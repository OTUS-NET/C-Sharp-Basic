### Цель
    
Добавление нотификаций в приложение, разработанное в предыдущих домашних заданиях:

- Отправлять нотификации пользователям через фоновые задачи
- Добавлять новые фоновые задачи
- Создавать регулярные нотификации

---

### Описание

Ссылка на [GitHub](https://github.com/OTUS-NET/C-Sharp-Basic/blob/main/Homeworks/16%20%D0%9D%D0%BE%D1%82%D0%B8%D1%84%D0%B8%D0%BA%D0%B0%D1%86%D0%B8%D0%B8/Task.md)

Перед выполнением нужно ознакомится с [Правила отправки домашнего задания на проверку](https://github.com/OTUS-NET/C-Sharp-Basic/blob/main/Homeworks/README.md)

1. Добавление сущности `Notification`
    - Создать класс `Notification` в Core/Entities. Свойства:
        - Guid Id
        - ToDoUser User
        - string Type //Тип нотификации. Например: DeadLine_{ToDoItem.Id}, Today_{DateOnly.FromDateTime(DateTime.UtcNow)}
        - string Text //Текст, который будет отправлен
        - DateTime ScheduledAt //Запланированная дата отправки
        - bool IsNotified //Флаг отправки
        - DateTime? NotifiedAt //Фактическая дата отправки
    - Создать класс `NotificationModel` в Infrastructure/DataAccess/Models. Разметить атрибутами Linq2Db аналогично другим моделям
    - Добавить свойство ITable<NotificationModel> Notifications в ToDoDataContext
    - Создать таблицу Notification в БД. Добавить индекс для UserId
2. Добавление `NotificationService`
    - Создать интерфейс `INotificationService` в папке Core/Services
    ```csharp
    public interface INotificationService
    {
        //Создает нотификацию. Если запись с userId и type уже есть, то вернуть false и не добавлять запись, иначе вернуть true
        Task<bool> ScheduleNotification(
            Guid userId,
            string type,
            string text,
            DateTime scheduledAt,
            CancellationToken ct);

        //Возвращает нотификации, у которых IsNotified = false && ScheduledAt <= scheduledBefore
        Task<IReadOnlyList<Notification>> GetScheduledNotification(DateTime scheduledBefore, CancellationToken ct);

        Task MarkNotified(Guid notificationId, CancellationToken ct);
    }
    ```
    - Создать класс `NotificationService`, который реализует интерфейс `INotificationService`. В конструкторе принимает IDataContextFactory<ToDoDataContext> factory и напрямую работает с БД юез репозиториев. Разместить в папке Infrastructure
3. Добавление `NotificationBackgroundTask`. Задача будет отправлять запланированные нотификации
    - Создать класс `NotificationBackgroundTask` в папке BackgroundTasks
    - Наследоваться от BackgroundTask(TimeSpan.FromMinutes(1), nameof(NotificationBackgroundTask)). Задача будет запускаться раз в минуту
    - Аргументы конструктора:
        - INotificationService notificationService
        - ITelegramBotClient bot
    - Реализовать метод Execute
        - Получить нотификации, которые нужно отправить. await notificationService.GetScheduledNotification(DateTime.UtcNow, ct);
        - Отправить нотификации через ITelegramBotClient
        - Пометить нотификации отправленными MarkNotified
    - Добавить фоновую задачу через `AddTask` в `BackgroundTaskRunner`
4. Добавление `DeadlineBackgroundTask`. Задача будет добавлять нотификации по задачам, у которых прошел Deadline 
    - Добавить метод в `IToDoRepository`. Возвращает активные задачи, у которых Deadline >= from && Deadline < to
        - Task<IReadOnlyList<ToDoItem>> GetActiveWithDeadline(Guid userId, DateTime from, DateTime to, CancellationToken ct);
    - Добавить метод в `IUserRepository`. Возвращает всех пользователей
        - Task<IReadOnlyList<ToDoUser>> GetUsers(CancellationToken ct);
    - Создать класс `DeadlineBackgroundTask` в папке BackgroundTasks
    - Наследоваться от BackgroundTask(TimeSpan.FromHours(1), nameof(DeadlineBackgroundTask)). Задача будет запускаться раз в час
    - Аргументы конструктора:
        - INotificationService notificationService
        - IUserRepository userRepository
        - IToDoRepository toDoRepository
    - Реализовать метод Execute
        - Получить список всех пользователей
        - Для каждого пользователя получить набор просроченных задач. await toDoRepository.GetActiveWithDeadline(user.UserId, DateTime.UtcNow.AddDays(-1).Date, DateTime.UtcNow.Date, ct);
        - Для каждой задачи создать нотификацию с типом $"Dealine_{task.Id}" и текстом $"Ой! Вы пропустили дедлайн по задаче {task.Name}"
    - Добавить фоновую задачу через `AddTask` в `BackgroundTaskRunner`
5. Добавление `TodayBackgroundTask`. Задача будет добавлять нотификации по задачам, которые запланированы на сегодня
    - Создать класс `TodayBackgroundTask` в папке BackgroundTasks
    - Наследоваться от BackgroundTask(TimeSpan.FromDays(1), nameof(TodayBackgroundTask)). Задача будет запускаться раз в день
    - Аргументы конструктора:
        - INotificationService notificationService
        - IUserRepository userRepository
        - IToDoRepository toDoRepository
    - Реализовать метод Execute
        - Получить список всех пользователей
        - Для каждого пользователя получить набор задач на сегодня
        - Для каждой задачи создать нотификацию с типом $"Today_{DateOnly.FromDateTime(DateTime.UtcNow)}". В тексте перечислить списком задач
    - Добавить фоновую задачу через `AddTask` в `BackgroundTaskRunner`

---

### Критерии оценивания

- Пункт 1 - 2 балла
- Пункт 2 - 2 балла
- Пункт 3 - 2 балла
- Пункт 4 - 2 балла
- Пункт 5 - 2 балл

Для зачёта домашнего задания достаточно 8 баллов.