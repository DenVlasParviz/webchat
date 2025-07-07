using Microsoft.AspNetCore.Identity;
using Web_chat.DTos.Records;
using Web_chat.Models;

namespace Web_chat.EndPoints;

public static class AuthServiceEndPoint
{
    public static RouteGroupBuilder MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("auth");
//POST  localhost:5146/auth/signup - передача данных с сайта vue signup логин и пароль(для регистрации)
        group.MapPost("signup", async(
            SignUpDtos dto,
            UserManager<User> userManager,
            SignInManager<User> signInManager
        ) =>
        {
            // 1) Проверяем, что логин свободен
            if (await userManager.FindByNameAsync(dto.Username) is not null)
                return Results.BadRequest("Username is already taken");

            // 2) Создаём сущность User
            var user = new User { UserName = dto.Username /*, Email = dto.Email*/ };

            // 3) UserManager создаст пользователя и захеширует пароль
            var createResult = await userManager.CreateAsync(user, dto.Password);
            if (!createResult.Succeeded)
                return Results.BadRequest(createResult.Errors);

            // 4) (Опционально) — сразу залогиним под этим пользователем
            await signInManager.SignInAsync(user, isPersistent: false);

            // 5) Возвращаем 201 с информацией
            return Results.Created($"/users/{user.Id}", new { user.Id, user.UserName });
        });
        group.MapPost("signin", async (
            SignInDtos dto,
            UserManager<User> userManager,
            SignInManager<User> signInManager
        ) =>
        {
            var user = await userManager.FindByNameAsync(dto.Username);
            if (user is null)
                return Results.BadRequest("Invalid username or password");
            var result = await signInManager.PasswordSignInAsync(dto.Username, dto.Password, dto.RememberMe, false);
            if (!result.Succeeded)
                return Results.BadRequest("Invalid username or password");
            return Results.Ok(new { message = "Login successful" });
        });

        return group;
    }
}