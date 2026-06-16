# Миграция: Telegram → Web API

Цепочка ДЗ ToDo переведена с Telegram-бота на **ASP.NET Core Web API (Controllers)**.

## Основные изменения

| Было | Стало |
|------|--------|
| Telegram Bot, ConsoleBot | Web API Controllers |
| ДЗ 08 Telegram | ДЗ 09 Web API + DI |
| ДЗ 10 Сценарии | Удалено |
| ДЗ 16 Нотификации | Удалено |
| `BackgroundTaskRunner` | `IHostedService` |

Чтобы перейти на новый формат ДЗ нужно в курс добавить ASP.NET Core Web API (DI + Swagger) вместо Telegram
