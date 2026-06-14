### Цель

Расширение Web API приложения:

- Сущность `ToDoList`
- CRUD списков через Controllers
- Пагинация задач (LINQ)

---

### Описание

Перед выполнением нужно ознакомиться с [Правила отправки домашнего задания на проверку](https://github.com/OTUS-NET/C-Sharp-Basic/blob/main/Homeworks/README.md)

1. Добавление сущности `ToDoList`
    - Добавить класс `ToDoList` по пути Core/Entities
        - Свойства
            - Guid Id
            - string Name
            - ToDoUser User
            - DateTime CreatedAt
    - Добавить свойство ToDoList? List в `ToDoItem`
    - Добавить аргумент ToDoList? list в `IToDoService.Add`
2. Добавление репозитория для `ToDoList`
    - Добавить интерфейс `IToDoListRepository`
    ```csharp
    public interface IToDoListRepository
    {
        //Если списка нет, то возвращает null
        Task<ToDoList?> Get(Guid id, CancellationToken ct);
        Task<IReadOnlyList<ToDoList>> GetByUserId(Guid userId, CancellationToken ct);
        Task Add(ToDoList list, CancellationToken ct);
        Task Delete(Guid id, CancellationToken ct);
        //Проверяет, есть ли у пользователя список с таким именем
        Task<bool> ExistsByName(Guid userId, string name, CancellationToken ct);
    }
    ```
    - Создать класс `FileToDoListRepository`, который реализует интерфейс `IToDoListRepository`. Реализовать класс аналогично `FileUserRepository`
3. Добавление класса сервиса для `ToDoList`
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
        - Размер имени списка не может быть больше 10 символов
        - Название списка должно быть уникально в рамках одного ToDoUser
    - Добавить метод `Task<IReadOnlyList<ToDoItem>> GetByUserIdAndList(Guid userId, Guid? listId, CancellationToken ct);` в интерфейс `IToDoService` и реализовать его
4. Добавьте контроллер `ListsController` и соответствующие DTO-классы (CreateListRequest, ListResponse):
    - [Route("api/users/{userId:guid}/lists")]
    - `GET /api/users/{userId}/lists`
    - `GET /api/users/{userId}/lists/{listId}/tasks` — получить задачи списка
    - `POST /api/users/{userId}/lists`
    - `DELETE /api/users/{userId}/lists/{listId}`
    - В `TaskResponse` нужно добавить информацию о списке
---

### Критерии оценивания

- Пункт 1 — 2 балла
- Пункт 2 — 2 балла
- Пункт 3 — 2 балла
- Пункт 4 — 4 балла

Для зачёта домашнего задания достаточно 8 баллов.
