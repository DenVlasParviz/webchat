// Models/ConversationUser.cs
using System;
using Web_chat.Models;

namespace Web_chat.Models
{
    public class ConversationUser
    {
        // Внешний ключ на Conversation
        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; } = null!;

        // Внешний ключ на User (IdentityUser.Id имеет тип string по умолчанию)
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}