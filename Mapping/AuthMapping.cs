using Web_chat.DTos.Records;
using Web_chat.Models;

namespace Web_chat.Mapping;
public record SignUpDtos(string Username, string Password, string Email);

public static class AuthMapping
{
    public static User ToEntity(this SignUpDtos dto)
        => new User
        {
            UserName = dto.Username,
            Email    = dto.Email
        };

}