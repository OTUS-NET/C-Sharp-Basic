### Цель
    
Расширение функционала приложения, разработанного в предыдущих домашних заданиях:

- Добавление поддержки сценариев с сохранением промежуточного состояния
- Добавление сценария для создания задачи
- Работа с Dictionary

---

### Описание

Перед выполнением нужно ознакомится с [Правила отправки домашнего задания на проверку](https://github.com/OTUS-NET/C-Sharp-Basic/blob/main/Homeworks/README.md)

1. Добавление `ScenarioContext`
    - Добавить enum `ScenarioType` со значениями
        - None
        - AddTask
    - Добавить класс `ScenarioContext`
        - Свойства
            - long UserId //Id пользователя в Telegram
            - ScenarioType CurrentScenario
            - string? CurrentStep
            - Dictionary<string, object> Data
        - Конструктор `ScenarioContext(ScenarioType scenario)`
    - Добавить интерфейс 
    ```csharp
    public interface IScenarioContextRepository
    {
        Task<ScenarioContext?> GetContext(long userId, CancellationToken ct);
        Task SetContext(long userId, ScenarioContext context, CancellationToken ct);
        Task ResetContext(long userId, CancellationToken ct);
    }
    ```
    - Создать класс `InMemoryScenarioContextRepository`, который реализует интерфейс `IScenarioContextRepository`. В качестве хранилища использовать `Dictionary<long, ScenarioContext>`
    - Файлы размещать в папке `./TelegramBot/Scenarios`
2. Добавление поддержки сценариев
    - Добавить enum `ScenarioResult` со значениями
        - Transition - Переход к следующему шагу. Сообщение обработано, но сценарий еще не завершен
        - Completed - Сценарий завершен
    - Добавить интерфейс 
    ```csharp
    public interface IScenario
    {
        bool CanHandle(ScenarioType scenario);
        Task<ScenarioResult> HandleMessageAsync(ITelegramBotClient bot, ScenarioContext context, Update update, CancellationToken ct);
    }
    ```
3. Обновление `UpdateHandler`
    - Добавить в конструктор аргументы:
        - `IEnumerable<IScenario>` scenarios
        - IScenarioContextRepository contextRepository
    - Добавить метод `IScenario GetScenario(ScenarioType scenario)`, который возвращает соответствующий сценарий. Если сценарий не найден, то выбрасывать исключение.
    - Добавить метод `Task ProcessScenario(ScenarioContext context, Update update, CancellationToken ct)`
        - Получает сценарий через метод `GetScenario`
        - Вызывает метод `IScenario.HandleMessageAsync`
        - ЕСЛИ метод вернул ScenarioResult.Completed, TO вызвать `IScenarioContextRepository.ResetContext`
        - ИНАЧЕ вызвать `IScenarioContextRepository.SetContext`
    - В метод `HandleUpdateAsync` добавить получение `ScenarioContext` через `IScenarioContextRepository` перед обработкой команд.
        - ЕСЛИ `ScenarioContext` найден, ТО вызвать метод `ProcessScenario` и завершить обработку
4. Добавление `AddTaskScenario`
    - Добавить класс `AddTaskScenario`, который реализует интерфейс `IScenario` и в конструкторе принимает `IUserService` и `IToDoService`
    - Добавить обработку шагов сценария (`ScenarioContext.CurrentStep`) через switch case
        - case null
            - Получить `ToDoUser` и сохранить его в `ScenarioContext.Context`.
            - Отправить пользователю сообщение "Введите название задачи:"
            - Обновить `ScenarioContext.CurrentStep` на "Name"
            - Вернуть `ScenarioResult.Transition`
        - case "Name"
            - Вызвать `IToDoService.Add`. Передать `ToDoUser` из `ScenarioContext.Context` и name из сообщения
            - Вернуть `ScenarioResult.Completed`
    - Обновить обработку команды `/addtask` в `UpdateHandler`
        - При получении команды `/addtask` создать `ScenarioContext` c `ScenarioType.AddTask` и вызвать метод `ProcessScenario`
    - Добавить кнопку `/addtask` через класс `ReplyKeyboardMarkup`
5. Добавление команды `/cancel` для остановки сценариев
    - При получении команды `/cancel` нужно вызвать метод `IScenarioContextRepository.ResetContext`
    - Обрабатывать команду нужно до запуска `ProcessScenario`
    - При запуске сценария у пользователя должна быть доступна одна кнопка `/cancel` через `ReplyKeyboardMarkup`
    - После завершения или отмены сценария должны вернуться кнопки с командами (/addtask, /showtask и тд)
    - Обновить `/help`
6. Добавление Deadline в `ToDoItem`
    - Добавить свойство DateTime Deadline в `ToDoItem`
    - Добавить аргумент DateTime deadline в `IToDoService.Add`
    - Добавить заполнение Deadline в `AddTaskScenario` через отдельный шаг. Формат текста dd.MM.yyyy. 
    - Если пользователь введет дату в неверном формате, сценарий не должен прерваться и нужно еще раз запросить дату.

---

### Критерии оценивания

- Пункт 1 - 2 балла
- Пункт 2 - 2 балла
- Пункт 3 - 2 балла
- Пункт 4 - 2 балла
- Пункт 5 - 1 балл
- Пункт 6 - 1 балл

Для зачёта домашнего задания достаточно 8 баллов.
