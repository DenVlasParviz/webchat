using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web_chat.EndPoints;
using Web_chat.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Web_chat.Migrations;
using Web_chat.Models;
using User = Web_chat.Models.User;


var builder = WebApplication.CreateBuilder(args);


// 1. Читаем строку подключения из appsettings.json — сразу после создания builder
var connectionString =builder.Configuration.GetConnectionString( "ChatDatabase");

// 2. Регистрируем DbContext наряду с другими сервисами
builder.Services.AddDbContext<ChatContext>(options => options.UseNpgsql(connectionString));

//JWT токен 

var jwtSection = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSection["Key"]);


//формирование JWT с username





// 3. Регистрируем остальные сервисы
builder.Services
    .AddIdentity<User, IdentityRole>(opts =>
    {
        // Base
        opts.User.RequireUniqueEmail = false;  // или true, если нужно Email

        // Пароль
        opts.Password.RequiredLength = 6;
        opts.Password.RequireDigit = false;
        opts.Password.RequireLowercase = false;
        opts.Password.RequireUppercase = false;
        opts.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<ChatContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSection["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSection["Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role,
    };
    opt.Events = new JwtBearerEvents
    {
        OnMessageReceived = ctx =>
        {
            var accesToken = ctx.Request.Query["access_token"];
            var path = ctx.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accesToken) && path.StartsWithSegments("/hubs/chat"))
            {
                ctx.Token = accesToken;
            }

            return Task.CompletedTask;
        }
    };
});
builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.LoginPath = "/auth/login";
});
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost5173", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowLocalhost5173");

app.UseAuthentication();
    
app.UseAuthorization();
app.MapAuthEndpoints();
app.MapSignupEndpoints();
app.UseCors("AllowLocalhost5173");


// 4. Настраиваем маршрутизацию
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/hubs/chat").RequireAuthorization();
    endpoints.MapControllers();
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
).WithStaticAssets();
await app.MigrateDbAsync();

app.Run();