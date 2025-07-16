using Microsoft.AspNetCore.SignalR;
using Web_chat.Models;

namespace Web_chat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatContext _context;

        public ChatHub(ChatContext dbContext)
        {
            _context = dbContext;
        }
        public async Task JoinConversation(int conversationId)
            => await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());

        public async Task LeaveConversation(int conversationId)
            => await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId.ToString());


        public async Task SendMessage( string message, int conversationId)
        {
            var userId   = Context.UserIdentifier;
            var userName = Context.User.Identity?.Name ?? "Anonymous";

            try
            {     Console.WriteLine($"➡️ SendMessage called: {message}");

                var msg = new Message
                {
                    SenderId = userId,
                    Text = message,
                    Timestamp = DateTime.UtcNow,  // <= ОБЯЗАТЕЛЬНО укажи время
                    ConversationId = conversationId
                };

                _context.Messages.Add(msg);
                await _context.SaveChangesAsync();
                await Clients.All.SendAsync("ReceiveMessage", userName, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] SendMessage failed: {ex.Message}");
                throw;
            }
        }

    }
}