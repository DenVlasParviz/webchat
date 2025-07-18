using System.ComponentModel.DataAnnotations.Schema;

namespace Web_chat.Models;

public class Conversation
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<ConversationUser> Users { get; set; } = new List<ConversationUser>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    
    
}