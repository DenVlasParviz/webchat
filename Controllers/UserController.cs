using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_chat.DTos.Records;
using Web_chat.Models;

namespace Web_chat.Controllers;
[Authorize]
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly ChatContext _db;
    public UserController(ChatContext db)  // ← теперь конструктор UserController
    {
        _db = db;
    }
    // GET /api/users
    [HttpGet]
    public async Task<IEnumerable<UserDto>> GetAll()
    {
        // возвращаем всех, кроме текущего
        var meId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return await _db.Users
            .Where(u => u.Id != meId)
            .Select(u => new UserDto(u.Id, u.UserName!))
            .ToListAsync();
    }
}