### Цель
    
Подключение ORM для приложения, разработанного в предыдущих домашних заданиях:

- Проектировать механизм запуска фоновых задач
- Добавлять фоновые задачи
- Сбрасывать сценарии пользователей по таймауту

---

### Описание

Ссылка на [GitHub](https://github.com/OTUS-NET/C-Sharp-Basic/blob/main/Homeworks/14%20%D0%9F%D0%BE%D0%B4%D0%BA%D0%BB%D1%8E%D1%87%D0%B5%D0%BD%D0%B8%D0%B5%20ORM/Task.md)

Перед выполнением нужно ознакомится с [Правила отправки домашнего задания на проверку](https://github.com/OTUS-NET/C-Sharp-Basic/blob/main/Homeworks/README.md)

1. Построение механизма фоновых задач
    - Создать в проекте папку `BackgroundTasks`
    - Создать интерфейс `IBackgroundTask`
    ```csharp
    public interface IBackgroundTask
    {
        Task Start(CancellationToken ct);
    }
    ```
    - Создать класс `BackgroundTaskRunner`
    ```csharp
    public class BackgroundTaskRunner : IDisposable
    {
        private readonly ConcurrentBag<IBackgroundTask> _tasks = new();
        private Task? _runningTasks;
        private CancellationTokenSource? _stoppingCts;

        /// <summary>
        /// Регистрирует задачу для последующего запуска.
        /// </summary>
        /// <exception cref="InvalidOperationException">Tasks are already running</exception>
        public void AddTask(IBackgroundTask task)
        {
            if (_runningTasks is not null)
                throw new InvalidOperationException("Tasks are already running");

            _tasks.Add(task);
        }

        /// <summary>
        /// Запускает зарегистрированные задачи
        /// </summary>
        /// <exception cref="InvalidOperationException">Tasks are already running</exception>
        public void StartTasks(CancellationToken ct)
        {
            if (_runningTasks is not null)
                throw new InvalidOperationException("Tasks are already running");

            _stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(ct);

            // Отдельная обёртка для логирования и корректной обработки отмены
            static async Task RunSafe(IBackgroundTask task, CancellationToken ct)
            {
                try
                {
                    await task.Start(ct);
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    // нормально завершаемся при отмене
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error in {task.GetType().Name}: {ex}");
                }
            }

            // Собираем все таски в один
            _runningTasks = Task.WhenAll(_tasks.Select(t => RunSafe(t, _stoppingCts.Token)));
        }

        /// <summary>
        /// Останавливает запущенные задачи и и ожидает из завершения
        /// </summary>
        public async Task StopTasks(CancellationToken ct)
        {
            if (_runningTasks is null)
                return;

            try
            {
                _stoppingCts?.Cancel();
            }
            finally
            {
                await _runningTasks.WaitAsync(ct).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
            }
        }

        public void Dispose()
        {
            _stoppingCts?.Cancel();
            _stoppingCts?.Dispose();
        }
    }
    ```
2. Добавление вспомогательного класса `BackgroundTask`
    - Добавить абстрактный класс `BackgroundTask`. Он нужен для переиспользоваия общего кода по запуску фоновых задач. От него будут наследоваться фоновые задачи.
    ```csharp
    public abstract class BackgroundTask(TimeSpan delay, string name) : IBackgroundTask
    {
        protected abstract Task Execute(CancellationToken ct);

        public async Task Start(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    Console.WriteLine($"{name}. Execute");
                    await Execute(ct);

                    Console.WriteLine($"{name}. Start delay {delay}");
                    await Task.Delay(delay, ct);
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    // нормально завершаемся при отмене
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{name}. Error: {ex}");
                    await Task.Delay(TimeSpan.FromSeconds(1), ct);
                }
            }
        }
    }
    ```
3. Изменение `ScenarioContext`
    - Добавить свойство public DateTime CreatedAt { get; } в `ScenarioContext`
    - Заполнять его нужно при создании `ScenarioContext` как DateTime.UtcNow
    - Добавить метод Task<IReadOnlyList<ScenarioContext>> GetContexts(CancellationToken ct); в `IScenarioContextRepository`
    - Реализовать метод GetContexts. Он должен возвращать все контексты
4. Добавление `ResetScenarioBackgroundTask`
    - Создать класс `ResetScenarioBackgroundTask` в папке BackgroundTasks
    - Наследоваться от BackgroundTask(TimeSpan.FromHours(1), nameof(ResetScenarioBackgroundTask)). Задача будет запускаться раз в час
    - Аргументы конструктора:
        - TimeSpan resetScenarioTimeout //Через какое время после создания отменять сценарий. Будем использовать 1 час
        - IScenarioContextRepository scenarioRepository
        - ITelegramBotClient bot
    - Реализовать метод Execute
        - Использовать scenarioRepository.GetContexts
        - Проверять время контекста
        - Вызывать scenarioRepository.ResetContext
        - Отправлять сообщение пользователю с текстом $"Сценарий отменен, так как не поступил ответ в течение {resetScenarioTimeout}" и клавиатурой ["/addtask", "/show", "/report"]
5. Запуск фоновых задач
    - Добавить в `Program.cs` создание `BackgroundTaskRunner`
    - Добавить нужные фоновые задачи (`ResetScenarioBackgroundTask`) через `AddTask`
    - Запустить фоновые задачи через `StartTasks` до запуска телеграм бота `botClient.StartReceiving`
    - Добавить остановку фоновых задач через `StopTasks` до отмены `CancellationTokenSource`
---

### Критерии оценивания

- Пункт 1 - 2 балла
- Пункт 2 - 2 балла
- Пункт 3 - 2 балла
- Пункт 4 - 2 балла
- Пункт 5 - 2 балл

Для зачёта домашнего задания достаточно 8 баллов.