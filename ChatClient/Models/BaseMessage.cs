namespace ChatClient.Models;

public abstract class BaseMessage
{
    public DateTime Timestamp { get; set; }
    public string Sender { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;

    protected BaseMessage()
    {
        Timestamp = DateTime.Now;
    }

    public override string ToString()
    {
        return $"[{Timestamp:HH:mm:ss}] {Sender}: {Text}";
    }
}