using Microsoft.EntityFrameworkCore;
using Web_chat.EndPoints;
using Web_chat.Hubs;
using Web_chat.Migrations;
using Web_chat.Models;


var builder = WebApplication.CreateBuilder(args);

// 1. Читаем строку подключения из appsettings.json — сразу после создания builder
var connectionString =builder.Configuration.GetConnectionString( "ChatDatabase");
Console.WriteLine("Существует ли файл chat.db?");
Console.WriteLine(File.Exists("chat.db")); // должен вывести True

Console.WriteLine("Полный путь: " + Path.GetFullPath("chat.db"));


// 2. Регистрируем DbContext наряду с другими сервисами
builder.Services.AddSqlite<ChatContext>(connectionString);

// 3. Регистрируем остальные сервисы
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapChatEndpoints();

// 4. Настраиваем маршрутизацию
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/hubs/chat");
    endpoints.MapControllers();
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
).WithStaticAssets();
await app.MigrateDbAsync();

app.Run();