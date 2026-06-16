### Цель
    
Расширение функционала приложения, разработанного в предыдущих домашних заданиях:

- Добавление сущности ToDoList
- Расширение сценария создания задачи
- Добавление сценариев для создания и удаления списков
- Работа с CallbackQuery
- Работа с ConcurrentDictionary

---

### Описание

Ссылка на [GitHub](https://github.com/OTUS-NET/C-Sharp-Basic/blob/main/Homeworks/11%20%D0%A1%D0%BF%D0%B8%D1%81%D0%BA%D0%B8%20%D0%B4%D0%BB%D1%8F%20%D0%B7%D0%B0%D0%B4%D0%B0%D1%87/Task.md)

Перед выполнением нужно ознакомиться с [Правила отправки домашнего задания на проверку](https://github.com/OTUS-NET/C-Sharp-Basic/blob/main/Homeworks/README.md)

В данном ДЗ добавляется поддержка списков для задач. Удобный функционал, чтобы сгуппировать задачи по смыслу.

![Демонстрация работы бота](Demo11.gif)

1. В `InMemoryScenarioContextRepository` заменить использование `Dictionary` на `ConcurrentDictionary`
2. Добавление сущности `ToDoList`
    - Добавить класс `ToDoList` по пути Core/Entities
        - Свойства
            - Guid Id
            - string Name
            - ToDoUser User
            - DateTime CreatedAt
    - Добавить свойство ToDoList? List в `ToDoItem`
    - Добавить аргумент ToDoList? list в `IToDoService.Add`
3. Добавление класса сервиса и репозитория для `ToDoList`
    - Добавить интерфейс `IToDoListRepository`
    ```csharp
    public interface IToDoListRepository
    {
        //Если спика нет, то возвращает null
        Task<ToDoList?> Get(Guid id, CancellationToken ct);
        Task<IReadOnlyList<ToDoList>> GetByUserId(Guid userId, CancellationToken ct);
        Task Add(ToDoList list, CancellationToken ct);
        Task Delete(Guid id, CancellationToken ct);
        //Проверяет, если ли у пользователя список с таким именем
        Task<bool> ExistsByName(Guid userId, string name, CancellationToken ct);
    }
    ```
    - Создать класс `FileToDoListRepository`, который реализует интерфейс `IToDoListRepository`. Реализовать класс аналогично `FileUserRepository`
    - Добавить интерфейс `IToDoListService`
    ```csharp
    public interface IToDoListService
    {
        Task<ToDoList> Add(ToDoUser user, string name, CancellationToken ct);
        Task<ToDoList?> Get(Guid id, CancellationToken ct);
        Task Delete(Guid id, CancellationToken ct);
        Task<IReadOnlyList<ToDoList>> GetUserLists(Guid userId, CancellationToken ct);
    }
    ```
    - Создать класс `ToDoListService`, который реализует интерфейс `IToDoListService`
        - Размер имени списка не может быть больше 10 символом
        - Название списка должно быть уникально в рамках одного ToDoUser
    - Добавить метод `Task<IReadOnlyList<ToDoItem>> GetByUserIdAndList(Guid userId, Guid? listId, CancellationToken ct);` в интерфейс `IToDoService` и реализовать его
4. Изменение команды /showtasks
    - Добавить `IToDoListService` в `UpdateHandler` через конструктор.
    - Переименовать команду /showtasks в /show
    - Команду /showalltasks удалить
    - Создать Dto классы с помощью которых мы будет работать с `CallbackQuery`. Разместить их в `TelegramBot/Dto`
        - Класс `CallbackDto`. Общий класс в котором есть `Action`
            Свойства:
                - string Action //с помощью него будет определять за какое действие отвечает кнопка
            Методы:
                - public static CallbackDto FromString(string input) //На вход принимает строку ввида "{action}|{prop1}|{prop2}...". Нужно создать CallbackDto с Action = action. Нужно учесть что в строке может не быть |, тогда всю строку сохраняем в Action.
                - public override string ToString() - переопределить метод. Он должен возвращать Action
        - Класс `ToDoListCallbackDto`. Наследовать от CallbackDto. Помимо `Action`, есть `ToDoListId`
            Свойства:
                -  Guid? ToDoListId
            Методы:
                - public static new ToDoListCallbackDto FromString(string input) //На вход принимает строку ввида "{action}|{toDoListId}|{prop2}...". Нужно создать ToDoListCallbackDto с Action = action и ToDoListId = toDoListId.
                - public override string ToString() - переопределить метод. Он должен возвращать $"{base.ToString()}|{ToDoListId}"
    - При получении команды /show нужно отправлять сообщение с текстом "Выберите список" и кнопками InlineKeyboardButton (см. Демонстрация работы бота)
        - Для этого нужно использовать класс `InlineKeyboardMarkup` и добавлять в него кнопки с помощью `InlineKeyboardButton.WithCallbackData(string text, string callbackData)`
        - Максимальный размер callbackData составляет 64 символа, поэтому в классах `CallbackDto` мы будем использовать компактный формат приведение к строкам
        - Для "📌Без списка" в callbackData пишем ToDoListCallbackDto.ToString(). Action = "show", ToDoListId = null
        - Для остальных списков в callbackData пишем ToDoListCallbackDto.ToString(). Action = "show", ToDoListId = Id
        - Для "🆕Добавить" в callbackData пишем "addlist". Для "❌Удалить" в callbackData пишем "deletelist"
5. Обработка нажатия на кнопки
    - В UpdateHandler добавить обработки нажатия на Inline кнопки. За это отвечает update.CallbackQuery. Пример:
    ```csharp
    await (update switch
    {
        { Message: { } message } => OnMessage(update, message, ct),
        { CallbackQuery: { } callbackQuery } => OnCallbackQuery(update, callbackQuery, ct),
        _ => OnUnknown(update)
    });
    ```
    - В `OnCallbackQuery` добавить проверку на то, что пользователь зарегистрирован. Незарегистрированным пользователям `CallbackQuery` не обрабатываем. Также нужно проверять запущен ли для пользователя сценарий и вызывать `ProcessScenario`
    - Чтобы получать обновления по `CallbackQuery` нужно добавить его в ReceiverOptions.AllowedUpdates
    - При получении `CallbackQuery` создаем `CallbackDto` с помощью CallbackDto.FromString(query.Data)
    - ЕСЛИ Action равен
        - "show" TO получить `ToDoListCallbackDto` и вернуть задачи, которые привязаны к списку ToDoListCallbackDto.ToDoListId
6. Добавление и удаление списка
    - Добавить `AddList` в `ScenarioType`
    - Добавить класс `AddListScenario`, который реализует интерфейс `IScenario` и в конструкторе принимает `IUserService` и `IToDoListService`
    - Добавить обработку шагов сценария (`ScenarioContext.CurrentStep`) через switch case
        - case null
            - Получить `ToDoUser` и сохранить его в `ScenarioContext.Data`.
            - Отправить пользователю сообщение "Введите название списка:"
            - Обновить `ScenarioContext.CurrentStep` на "Name"
            - Вернуть `ScenarioResult.Transition`
        - case "Name"
            - Вызвать `IToDoListService.Add`. Передать `ToDoUser` из `ScenarioContext.Data` и name из сообщения
            - Вернуть `ScenarioResult.Completed`
    - При нажатии на кнопку "🆕Добавить" должен запускаться сценарий `AddListScenario`
    - Добавить `DeleteList` в `ScenarioType`
    - Добавить класс `DeleteListScenario`, который реализует интерфейс `IScenario` и в конструкторе принимает `IUserService`, `IToDoListService` и `IToDoService`
    - Добавить обработку шагов сценария (`ScenarioContext.CurrentStep`) через switch case
        - case null
            - Получить `ToDoUser` и сохранить его в `ScenarioContext.Data`.
            - Отправить пользователю сообщение "Выберете список для удаления:" с Inline кнопками. callbackData = ToDoListCallbackDto.ToString(). Action = "deletelist"
            - Обновить `ScenarioContext.CurrentStep` на "Approve"
        - case "Approve"
            - Получить `ToDoList` и сохранить его в `ScenarioContext.Data`.
            - Отправить пользователю сообщение "Подтверждаете удаление списка {toDoList.Name} и всех его задач" с Inline кнопками: WithCallbackData("✅Да", "yes"), WithCallbackData("❌Нет", "no")
            - Обновить `ScenarioContext.CurrentStep` на "Delete"
        - case "Delete"
            - ЕСЛИ update.CallbackQuery.Data равна
            - "yes" ТО удалить все задачи по `ToDoUser` и `ToDoList`. Удалить `ToDoList`. 
            - "no" ТО отправить сообщение "Удаление отменено".
            - Вернуть `ScenarioResult.Completed`.
    - При нажатии на кнопку "❌Удалить" должен запускаться сценарий `DeleteListScenario`
7. Добавить выбор списка в сценарий `AddTaskScenario`
    - Добавить заполнение `ToDoList` в `AddTaskScenario` через отдельный шаг
    - Выбирать список нужно через Inline кнопки (см. Демонстрация работы бота)
    - Обработка update.CallbackQuery должна быть внутри `AddTaskScenario`
8. Вне сценариев пользователю должны быть доступны кнопки ["/addtask", "/show", "/report"] через `ReplyKeyboardMarkup`.
9. Обновить /help

Примеры работы с Telegram API: https://github.com/TelegramBots/Telegram.Bot.Examples

---

### Критерии оценивания

- Пункты 1-3 - 2 балла
- Пункт 4 - 2 балла
- Пункт 5 - 2 балла
- Пункт 6 - 2 балла
- Пункт 7 - 1 балл
- Пункты 8-9 - 1 балл

Для зачёта домашнего задания достаточно 8 баллов.
