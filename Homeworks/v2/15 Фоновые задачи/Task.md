### Цель

Добавление фоновых задач в Web API приложение:

- `IHostedService` / `BackgroundService`
- Мягкое удаление задач
- Физическая очистка устаревших записей

---

### Описание

Перед выполнением нужно ознакомиться с [Правила отправки домашнего задания на проверку](https://github.com/OTUS-NET/C-Sharp-Basic/blob/main/Homeworks/README.md)

**До этого ДЗ** удаление задач было **физическим**. В этом ДЗ меняется модель удаления.

1. Расширить `ToDoItemState`:
    ```csharp
    public enum ToDoItemState { Active, Completed, Deleted }
    ```
2. Перевести `IToDoService.Delete` и `DELETE /api/tasks/{taskId}` на **мягкое удаление**:
    - `State = Deleted`
    - `StateChangedAt = DateTime.UtcNow`
    - Запись остаётся в хранилище
3. Обновить выборки — **не возвращать** задачи в статусе `Deleted`.
4. Добавить в `IToDoRepository`:
    ```csharp
    Task<IReadOnlyList<ToDoItem>> GetDeletedOlderThan(TimeSpan age, CancellationToken ct);
    Task Purge(Guid id, CancellationToken ct);
    ```
5. Создать `DeletedTasksCleanupHostedService` (`BackgroundService`):
    - Документация https://learn.microsoft.com/ru-ru/aspnet/core/fundamentals/host/hosted-services
    - Раз в сутки (`TimeSpan.FromHours(24)`)
    - Через `IServiceScopeFactory` получить `IToDoRepository`
    - Найти задачи: `State == Deleted` и `StateChangedAt < UtcNow.AddDays(-90)`
    - Вызвать `Purge` для каждой
6. Зарегистрировать в `Program.cs`:
    ```csharp
    builder.Services.AddHostedService<DeletedTasksCleanupHostedService>();
    ```

---

### Критерии оценивания

- Пункты 1–3 — 4 балла
- Пункт 4 — 2 балла
- Пункты 5–6 — 4 балла

Для зачёта домашнего задания достаточно 8 баллов.
