using Microsoft.EntityFrameworkCore;
using Web_chat.Models;

namespace Web_chat.Migrations;

public static class DataExtensions
{
    public static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope =app.Services.CreateScope();
        var dbcontext = scope.ServiceProvider.GetRequiredService<ChatContext>();
        await dbcontext.Database.MigrateAsync();
    }
}