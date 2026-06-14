### Цель

Расширение функционала приложения, разработанного в предыдущих домашних заданиях:

- Создание Web API на ASP.NET Core
- Знакомство с Dependency Injection
- API Controllers
- Swagger

---

### Описание

Перед выполнением нужно ознакомиться с [Правила отправки домашнего задания на проверку](https://github.com/OTUS-NET/C-Sharp-Basic/blob/main/Homeworks/README.md)

1. Создайте проект `Otus.ToDo.Api` (ASP.NET Core Web API).
2. Перенесите папки `Core` и `Infrastructure` из предыдущих ДЗ (сервисы, репозитории, сущности).
3. Зарегистрировать сервисы и репозитории в DI.
4. Создайте **Controllers** :
   - `UsersController` — регистрация пользователя, отчёт. [Route("api/users")]
   - `TasksController` — задачи (Add, list, complete, delete, find). [Route("api/users/{userId:guid}/tasks")]
   - Отдельные DTO классы для API положить в папку `Dto/`:
      - CreateUserRequest, UserResponse
      - CreateTaskRequest, TaskReponse
      - Набор свойств для этих классов определите при реализации пункта 5
5. Реализуйте endpoints:

| Бывшая команда | HTTP |
|----------------|------|
| `/start` | `POST /api/users` — body: `{ "userName": "..." }` |
| `/addtask` | `POST /api/users/{userId}/tasks` |
| `/showtasks` | `GET /api/users/{userId}/tasks?state=active` |
| `/showalltasks` | `GET /api/users/{userId}/tasks` |
| `/completetask` | `PATCH /api/tasks/{taskId}/complete` |
| `/removetask` | `DELETE /api/tasks/{taskId}` |
| `/report` | `GET /api/users/{userId}/report` |
| `/find` | `GET /api/users/{userId}/tasks?prefix=...` |

6. Подключите Swagger (`AddSwaggerGen`, `UseSwaggerUI`).
7. Обрабатывайте доменные исключения (`TaskCountLimitException`, `DuplicateTaskException` и т.д.) — возвращайте `Bad Request` и `Conflict` с сообщением.

---

### Критерии оценивания

- Пункт 1 - 1 балл
- Пункт 2 - 1 балл
- Пункт 3 - 1 балл
- Пункт 4 - 2 балла
- Пункт 5 - 2 балла
- Пункт 6 - 2 балл
- Пункт 7 - 1 балл

Для зачёта домашнего задания достаточно 8 баллов.
