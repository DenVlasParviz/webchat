using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Web_chat.Models;
using Web_chat.DTos.Records;


namespace Web_chat.Controllers;
[Authorize]
[ApiController]
[Route("api/chats")]
public class ChatController :ControllerBase
{
   private readonly ChatContext _db;
   public ChatController(ChatContext db)
   {
       _db = db;
   }

   [HttpGet]
   public async Task<IEnumerable<ConversationDtos>> GetConversation()
   {
       var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
       return await _db.ConversationUsers.Where(cu => cu.UserId == userId)
           .Select(cu => new ConversationDtos(cu.Conversation.Id, cu.Conversation.Name)).ToListAsync();
   }
   
   // GET api/chats/[conversationId]/messages

   [HttpGet("{conversationId}/messages")]
   public async Task<IEnumerable<ChatMessagesDtos>> GetMessages(int conversationId)
   {
       return await _db.Messages.Where(m => m.ConversationId == conversationId).OrderBy(m => m.Timestamp)
           .Select(m => new ChatMessagesDtos(m.Id, m.Sender!.UserName, m.Text, m.Timestamp)).ToListAsync();
       
   }
   [HttpPost]
   public async Task<ConversationDtos> CreateConversation([FromBody] string name)
   {
       var conv = new Conversation { Name = name };
       _db.Conversations.Add(conv);
       await _db.SaveChangesAsync();
       return new ConversationDtos(conv.Id, conv.Name);
   }

   // 4) (Опционально) — добавить пользователя в беседу
   [HttpPost("{conversationId}/users/{userId}")]
   public async Task<IActionResult> AddUser(int conversationId, string userId)
   {
       _db.ConversationUsers.Add(new ConversationUser {
           ConversationId = conversationId,
           UserId         = userId
       });
       await _db.SaveChangesAsync();
       return NoContent();
   }

   [HttpGet("last-messages")]
   public async Task<IEnumerable<LastMessageDto>> GetLastMessages()
   {
     
       var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

       var conversationIds = await _db.ConversationUsers
           .Where(cu => cu.UserId == userId)
           .Select(cu => cu.ConversationId)
           .ToListAsync();

       var list = await _db.LastMessages
           .Where(v => conversationIds.Contains(v.ConversationId))
           .ToListAsync();

       return list
           .Select(v => new LastMessageDto(
               v.ConversationId,
               v.SenderId,
               v.Text,
               v.Timestamp))
           .ToList();
   }
   

}