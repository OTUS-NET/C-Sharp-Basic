### Цель
    
Расширение функционала приложения, разработанного в предыдущих домашних заданиях:

- Добавление поддержки сценариев с сохранением промежуточного состояния
- Добавление сценария для создания задачи
- Работа с Dictionary

---

### Описание

Ссылка на [GitHub](https://github.com/OTUS-NET/C-Sharp-Basic/blob/main/Homeworks/10%20%D0%A1%D1%86%D0%B5%D0%BD%D0%B0%D1%80%D0%B8%D0%B8/Task.md)

Перед выполнением нужно ознакомится с [Правила отправки домашнего задания на проверку](https://github.com/OTUS-NET/C-Sharp-Basic/blob/main/Homeworks/README.md)

В данном ДЗ добавляется поддержка сценариев - это последовательный набор действий с сохранением промежуточного состояния. Они нужны для обработки цепочек действий, поддержки диалога и улучшения UX.

![Демонстрация работы бота](Demo10.gif)

1. Добавление `ScenarioContext`. Класс, который будет хранить информацию о контексте(сессии) пользователя. 
    - Добавить enum `ScenarioType`. В нем будем хранить все поддерживаемые сценарии. Значениями:
        - None
        - AddTask
    - Добавить класс `ScenarioContext`
        - Свойства
            - long UserId //Id пользователя в Telegram
            - ScenarioType CurrentScenario
            - string? CurrentStep //Текущий шаг сценария
            - Dictionary<string, object> Data //Дополнительная инфрмация, необходимая для работы сценария
        - Конструктор `ScenarioContext(ScenarioType scenario)`
    - Добавить интерфейс 
    ```csharp
    //Репозиторий, который отвечает за доступ к контекстам пользователей
    public interface IScenarioContextRepository
    {
        //Получить контекст пользователя
        Task<ScenarioContext?> GetContext(long userId, CancellationToken ct);
        //Задать контекст пользователя
        Task SetContext(long userId, ScenarioContext context, CancellationToken ct);
        //Сбросить (очистить) контекст пользователя
        Task ResetContext(long userId, CancellationToken ct);
    }
    ```
    - Создать класс `InMemoryScenarioContextRepository`, который реализует интерфейс `IScenarioContextRepository`. В качестве хранилища использовать `Dictionary<long, ScenarioContext>`
    - Файлы (классы и интерфейсы) размещать в папке `./TelegramBot/Scenarios`
2. Добавление поддержки сценариев
    - Добавить enum `ScenarioResult`. Нужен для получения результата выполнения сценария. Значениями:
        - Transition - Переход к следующему шагу. Сообщение обработано, но сценарий еще не завершен
        - Completed - Сценарий завершен
    - Добавить интерфейс `IScenario`. Нужен для определения логики работы сценариев
    ```csharp
    public interface IScenario
    {
        //Проверяет, может ли текущий сценарий обрабатывать указанный тип сценария.
        //Используется для определения подходящего обработчика в системе сценариев.
        bool CanHandle(ScenarioType scenario);
        //Обрабатывает входящее сообщение от пользователя в рамках текущего сценария.
        //Включает основную бизнес-логику
        Task<ScenarioResult> HandleMessageAsync(ITelegramBotClient bot, ScenarioContext context, Update update, CancellationToken ct);
    }
    ```
3. Обновление `UpdateHandler` для поддержки сценариев
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
            - Получить `ToDoUser` и сохранить его в `ScenarioContext.Data`.
            - Отправить пользователю сообщение "Введите название задачи:"
            - Обновить `ScenarioContext.CurrentStep` на "Name"
            - Вернуть `ScenarioResult.Transition`
        - case "Name"
            - Вызвать `IToDoService.Add`. Передать `ToDoUser` из `ScenarioContext.Data` и name из сообщения
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
