### Цель
    
Расширение функционала приложения, разработанного в предыдущих домашних заданиях:

- Добавление асинхронности

---

### Описание

Перед выполнением нужно ознакомится с [Правила отправки домашнего задания на проверку](https://github.com/OTUS-NET/C-Sharp-Basic/blob/main/Homeworks/README.md)

1. Подключение асинхронной библиотеки `Otus.ToDoList.ConsoleBot`
    - Добавить к себе в решение и в зависимости к своему проекту с ботом проект `Otus.ToDoList.ConsoleBot` [GitHub](https://github.com/OTUS-NET/C-Sharp-Basic/tree/main/Homeworks/07%20%D0%90%D1%81%D0%B8%D0%BD%D1%85%D1%80%D0%BE%D0%BD%D0%BD%D0%BE%D1%81%D1%82%D1%8C%2C%20%D0%B4%D0%B5%D0%BB%D0%B5%D0%B3%D0%B0%D1%82%D1%8B%20%D0%B8%20%D1%81%D0%BE%D0%B1%D1%8B%D1%82%D0%B8%D1%8F/Otus.ToDoList.ConsoleBot) вместо аналогичного проекта добавленнего в рамках ДЗ "ООП классы и интерфейсы"
    - Ознакомиться с README.md
    - Переключиться на использование асинхронных методов из библиотеки `Otus.ToDoList.ConsoleBot`. Обратите внимание на методы из `IUpdateHandler` и `ITelegramBotClient`
    - Реализовать метод `IUpdateHandler.HandleErrorAsync`. В нем нужно выводить информацию об ошибке в консоль
    - В метод `ITelegramBotClient.StartReceiving` нужно передавать `CancellationToken`. Его нужно создать с помощью `CancellationTokenSource`
    - Код библиотеки `Otus.ToDoList.ConsoleBot` не нужно изменять
2. Перевод интерфейсов на асинхронность
    - Мигрировать синхронные методы всех интерфейсов на асинхронные. Методы должны возвращать `Task` или `Task<>` и получать `CancellationToken`
    - `IUserService` `IUserRepository` `IToDoService` `IToDoRepository` и тд
---

### Критерии оценивания

- Пункт 1 - 7 баллов
- Пункт 2 - 3 балла

Для зачёта домашнего задания достаточно 6 баллов.
