### Цель
    
Подключение ORM для приложения, разработанного в предыдущих домашних заданиях:

- Использовать linq2db.PostgreSQL
- Описывать модель БД через классы
- Выполнять запросы в БД через linq2db

---

### Описание

Ссылка на [GitHub](https://github.com/OTUS-NET/C-Sharp-Basic/blob/main/Homeworks/14%20%D0%9F%D0%BE%D0%B4%D0%BA%D0%BB%D1%8E%D1%87%D0%B5%D0%BD%D0%B8%D0%B5%20ORM/Task.md)

Перед выполнением нужно ознакомится с [Правила отправки домашнего задания на проверку](https://github.com/OTUS-NET/C-Sharp-Basic/blob/main/Homeworks/README.md)

1. Подготовка моделей для описания структуры БД
   - Подключите в проект последную версию nuget пакета `linq2db.PostgreSQL`
    - Удалить все конструкторы и сделайте все свойства открытыми (get; set;) в классах в Core/Entities. Будем использовать анемичную модель, для этого нужны чистые классы
    - Создать классы модели для таблиц в Core/DataAccess/Models
        - `ToDoItemModel`
        - `ToDoListModel`
        - `ToDoUserModel`
    - Через атрибуты отпределите имена таблиц([Table("...")]), первичные ключи ([PrimaryKey]), колонки ([Column]), внешние ключи ([Association(ThisKey = nameof(), OtherKey = nameof())]). Подробную информацию можно найти в [документации](https://linq2db.github.io/#define-poco-class)
2. Создание DataContext
    - Создайте класс `ToDoDataContext` в Infrastructure/DataAccess. 
        - Наследуется от DataConnection
        - Конструктор public ToDoDataContext(string connectionString) : base(ProviderName.PostgreSQL, connectionString)
        - Иметь свойства, которые отражают таблицы в БД (ToDoUsers, ToDoLists, ToDoItems)
        - Пример из [документации](https://linq2db.github.io/#dataconnection-class)
    - Создать интерфейс IDataContextFactory<TDataContext>. Нужно чтобы создавать `DataConnection` на каждую сессию https://linq2db.github.io/articles/general/Managing-data-connection.html
    ```csharp
    public interface IDataContextFactory<TDataContext> where TDataContext : DataConnection
    {
        TDataContext CreateDataContext();
    }
    ```
    - Создать класс `DataContextFactory`, который реализует интерфейс `IDataContextFactory<ToDoDataContext>`
3. Создание маппера
    - Создать статический класс `ModelMapper` в Infrastructure/DataAccess. Через него будем делать маппинг между Infrastructure/DataAccess/Models и Core/DataAccess/Models
    ```csharp
    internal static class ModelMapper
    {
        public static ToDoUser MapFromModel(ToDoUserModel model);
        public static ToDoUserModel MapToModel(ToDoUser entity);
        public static ToDoItem MapFromModel(ToDoItemModel model);
        public static ToDoItemModel MapToModel(ToDoItem entity);
        public static ToDoList MapFromModel(ToDoListModel model);
        public static ToDoListModel MapToModel(ToDoList entity);
    }
    ```
    - Реализовать все методы
4. Реализация SqlToDoRepository
    - Создать класс `SqlToDoRepository`, который реализует интерфейс `IToDoRepository`
    - В конструкторе получать IDataContextFactory<ToDoDataContext> factory
    - Реализовать все методы интерфейса. Создавать dbContext в каждом методе. using var dbContext = factory.CreateDataContext();
    - Использовать `ModelMapper`
    - Не забудь добавлять LoadWith, чтобы загружать связанные сущности(eager loading)
    ```csharp
        .LoadWith(i => i.User)
        .LoadWith(i => i.List)
        .LoadWith(i => i.List!.User)
    ```
5. Аналогично создать класс `SqlToDoListRepository`, который реализует интерфейс `IToDoListRepository`
6. Аналогично создать класс `SqlUserRepository`, который реализует интерфейс `IUserRepository`
7. Добавление использовать SQL репозиториев
    - Добавить использование этих репозиториев вместо File реализаций
    - Указать connectionString до БД, которая разрабатывалась в ДЗ "Модель базы данных"

---

### Критерии оценивания

- Пункт 1 - 2 балла
- Пункт 2 - 2 балла
- Пункт 3 - 2 балла
- Пункт 4 - 1 балл
- Пункт 5 - 1 балл
- Пункт 6 - 1 балл
- Пункт 7 - 1 балл

Для зачёта домашнего задания достаточно 8 баллов.
