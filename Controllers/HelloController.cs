using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Web_chat.Controllers
{
    

[ApiController]
[Route("api/[controller]")]
public class HelloController: ControllerBase
{
   // GET api/hello
        [HttpGet]
        public IActionResult Get()
        {
            // Можно вернуть строку или JSON-объект
            return Ok(new { message = "Привет от ASP.NET Core!" });
        }
}
}