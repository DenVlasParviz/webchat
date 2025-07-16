namespace Web_chat.Models;

// Models/Message.cs
public class Message
{
    public int Id { get; set; }

    public int ConversationId { get; set; }
    public Conversation Conversation { get; set; } = null!;

    // теперь Nullable
    public string? SenderId { get; set; }
    public User? Sender { get; set; }

    public string Text { get; set; } = null!;
    public DateTime Timestamp { get; set; }
}
