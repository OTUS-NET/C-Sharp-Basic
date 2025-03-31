#pragma warning disable CS8618

namespace Otus.ToDoList.ConsoleBot.Types;
public class Message
{
    public int Id { get; init; }
    public string Text { get; init; }
    public Chat Chat { get; init; }
    public User From { get; init; }
}
