### Цель

Расширение функционала приложения, разработанного в предыдущих домашних заданиях:

- Работа с интерфейсами
- Выделение сервисного слоя

---

### Описание

Перед выполнением нужно ознакомиться с [Правила отправки домашнего задания на проверку](https://github.com/OTUS-NET/C-Sharp-Basic/blob/main/Homeworks/README.md)

1. Структура проекта
    ```
    Project/
    ├── Core/
    │   ├── Entities/
    │   ├── Services/
    │   └── Exceptions/
    └── Program.cs
    ```
2. Удалить команду `/echo`
3. Добавление класса сервиса `UserService`
    - Добавить интерфейс `IUserService`
    ```csharp
    interface IUserService
    {
        ToDoUser RegisterUser(string userName);
        ToDoUser? GetUser(Guid userId);
    }
    ```
    - Создать класс `UserService`, который реализует интерфейс `IUserService`
4. Изменение логики команды команды `/start`
    - Регистрировать пользователя нужно через `RegisterUser`
    - Для обработки команды нужно использовать `IUserService.GetUser`. Если пользователь не найден, то вызывать `IUserService.RegisterUser`
    - Если пользователь не зарегистрирован, то ему доступны только команды `/help` `/info`
5. Добавление класса сервиса `ToDoService`
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
6. Изменение команд `/addtask` `/removetask` `/completetask`
    - Добавить использование `IToDoService` при обработке команд
    - Изменить формат обработки команды `/addtask`. Нужно сразу передавать имя задачи в команде. Пример: `/addtask Новая задача`
    - Изменить формат обработки команды `/removetask`. Нужно сразу передавать Id задачи в команде. Пример: `/removetask ffbfe448-4b39-4778-98aa-1aed98f7eed8`
    - Изменить формат обработки команды `/completetask`. Нужно сразу передавать Id задачи в команде. Пример: `/completetask ffbfe448-4b39-4778-98aa-1aed98f7eed8`

Примечание: Можно заменить catch с разными типами исключений, если в них нет кастомной логики, на один catch(Exception ex). Так как в предыдущем задание сatch с разными типами исключений добавлялись в учебных целям и в реальных проектах не нужно делать catch на каждый тип исключения, если в них нет специальной логики.

---

### Критерии оценивания

- Пункт 1 - 1 балла
- Пункт 2 - 1 балл
- Пункт 3 - 2 балл
- Пункт 4 - 2 балла
- Пункт 5 - 2 балла
- Пункт 6 - 2 балла

Для зачёта домашнего задания достаточно 8 баллов.
