using Web_chat.DTos.Records;
using Web_chat.Hubs;
using Web_chat.Models;

namespace Web_chat.Mapping;

public static class ChatMapping
{
    public static ChatMessagesDtos ToChatMessagesDts(this Message message)
    {
        return new (message.Id,message.Sender.UserName,message.Text,message.Timestamp);
    } 
}