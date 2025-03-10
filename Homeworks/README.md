# Правила отправки домашнего задания на проверку

Для проверки домашнего задания нужно отправлять ссылку на Pull Request/Merge Request в любых удобных вам git сервисах (GitHub, GitVerse, GitLab, Azure DevOps и тд). Туда должны попадать изменения, которые относятся только к домашнему заданию, которое предоставляется на проверку. Для этого можно пользоваться следующим алгоритмом:

1.  **Создайте новую ветку для задания**
    - Перед началом работы над ДЗ создайте новую ветку от главной ветки (обычно `main` или `master`).
    - Давайте веткам понятные названия, отражающие суть задания (например, `feature/homework-3`).
2.  **Вносите изменения только в эту ветку**
    - Все файлы, которые относятся к данному заданию, меняйте и добавляйте только в новой ветке.
3.  **Создайте Pull Request (Merge Request)**
    - Когда задание выполнено, откройте Pull/Merge Request из созданной ветки в главную (`main/master`).
    - В PR/MR укажите краткое описание выполненной работы.
4.  **Отправьте ссылку на Pull/Merge Request на проверку**
    - Проверьте, что в PR/MR нет лишних коммитов и изменений, не относящихся к заданию.
5.  **Устраните замечания после ревью**
    - Если преподаватель оставит комментарии или обнаружит ошибки, внесите исправления в ту же ветку.
    - Все изменения автоматически подтянутся в ваш открытый Pull/Merge Request.
6.  **Закрыть Pull/Merge Request**
    - После принятия ДЗ преподавателем закрыть Pull Request/Merge Request и влить изменения в корневую ветку (`main/master`). Это особенно важно, если ДЗ связано с предыдущим.

---

### Рекомендации

- **Не забудьте обновить ветку** из `main/master` перед началом работы, чтобы избежать конфликтов.
- **Добавляйте понятные коммиты**: короткие и описательные сообщения облегчают понимание истории изменений.
- **Следите за конфликтами** и, при необходимости, своевременно их решайте.
- **Оформляйте PR/MR** аккуратно, чтобы проверяющему было проще разобраться в ваших изменениях.