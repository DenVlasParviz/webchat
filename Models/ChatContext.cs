using Microsoft.EntityFrameworkCore;

namespace Web_chat.Models;

    public class ChatContext(DbContextOptions<ChatContext> options) : DbContext(options)
    {
        // 1) Конструктор, принимающий DbContextOptions 
      

        public DbSet<Message> Messages => Set<Message>();
        }
        

        // 2) Набор сообщений
    
