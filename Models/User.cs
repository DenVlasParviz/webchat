using Microsoft.AspNetCore.Identity;

namespace Web_chat.Models
{
    public class User : IdentityUser
    {
        public ICollection<ConversationUser> Conversations { get; set; } = new List<ConversationUser>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        
        
        
    }
}