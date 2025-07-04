using Microsoft.AspNetCore.Mvc;
using Tesisatci.Dtos;

namespace Tesisatci.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto login)
        {
            // Sabit admin bilgileri
            var adminUsername = "Mekan";
            var adminPassword = "974412625";

            if (login.Username == adminUsername && login.Password == adminPassword)
            {
                return Ok(new { success = true });
            }
            else
            {
                return Unauthorized(new { success = false, message = "Geçersiz kullanıcı adı veya şifre" });
            }
        }
    }
}
