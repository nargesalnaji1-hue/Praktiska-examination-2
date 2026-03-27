namespace ChatClient.Models;

public class SystemEventMessage : BaseMessage
{
    public SystemEventMessage(string text)
    {
        Sender = "SYSTEM";
        Text = text;
    }
}
