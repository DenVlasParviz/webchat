using Microsoft.EntityFrameworkCore;
using Web_chat.Mapping;
using Web_chat.Models;

namespace Web_chat.EndPoints;

public static class ChatEndPoint
{
    private const string getChatEndPoint = "GetChat";

    public static RouteGroupBuilder MapChatEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("chat");
        group.MapGet("/", async (ChatContext dbContext) => await dbContext.Messages.Select(chat =>
            chat.ToChatMessagesDts()).ToListAsync());


        return group;
    }
}