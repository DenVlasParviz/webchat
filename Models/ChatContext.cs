using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Web_chat.Models
{
    public class ChatContext : IdentityDbContext<User>
    {
        public ChatContext(DbContextOptions<ChatContext> options)
            : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ConversationUser> ConversationUsers { get; set; }
        
        public DbSet<LastMessage> LastMessages { get; set; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            
            builder.Entity<ConversationUser>().HasKey(cu => new { cu.ConversationId, cu.UserId });
            
            builder.Entity<ConversationUser>().HasOne(cu => cu.Conversation).WithMany(c => c.Users).HasForeignKey(cu => cu.ConversationId);
            builder.Entity<ConversationUser>()
                .HasOne(cu => cu.User)
                .WithMany(u => u.Conversations)
                .HasForeignKey(cu => cu.UserId);

            // 4) Conversation ↔ Message
            builder.Entity<Message>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId);

            // 5) User ↔ Message
            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.SenderId) 
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<LastMessage>().ToView("LastMessages").HasNoKey();
        }
    }
}