using Microsoft.EntityFrameworkCore;

namespace Web_chat.Models;
[Keyless]
public class LastMessage
{
    public int Id { get; set; }
    public int ConversationId { get; set; }
    public int SenderId { get; set; }
    public string Text { get; set; }
    public DateTime Timestamp { get; set; }
}