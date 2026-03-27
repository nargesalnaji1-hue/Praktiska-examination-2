namespace ChatClient.Models;

public class ChatMessage : BaseMessage
{
    public ChatMessage(string sender, string text)
    {
        Sender = sender;
        Text = text;
    }
}
