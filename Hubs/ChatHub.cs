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

        public async Task SendMessage(string user, string message)
        {
            try
            {
                var msg = new Message
                {
                    Sender = user,
                    Text = message,
                    Timestamp = DateTime.UtcNow  // <= ОБЯЗАТЕЛЬНО укажи время
                };

                _context.Messages.Add(msg);
                await _context.SaveChangesAsync();
                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] SendMessage failed: {ex.Message}");
                throw;
            }
        }

    }
}