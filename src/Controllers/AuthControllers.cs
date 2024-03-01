using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using TaskManager.Schemas;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Database;
using Microsoft.EntityFrameworkCore;
using TaskManager.Database.Models; 

namespace TaskManager.Controllers; 

[Route("api/auth/[controller]")]
[ApiController]
public class RegisterUserController : ControllerBase
{
    private readonly TaskManagerContext _context;

    public RegisterUserController(TaskManagerContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserSchema model)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email || x.FullName == model.FullName);
        if (user != null) {
            return NotFound(new JsonResult("Такой пользватель уже существуют"));
        }
        User userCreate = new User
        {
            FullName = model.FullName,
            Email = model.Email
        }; 

        await _context.Users.AddAsync(userCreate);
        await _context.SaveChangesAsync(); 

        return new JsonResult(user);
    }
}

[Route("api/auth/[controller]")]
[ApiController]
public class LoginUserController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public LoginUserController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserSchema model)
    {
        // Здесь ваша логика аутентификации пользователя
        // Пример: проверка логина и пароля в базе данных

        // Проверка логина и пароля (пример)
        if (model.Email == "admin" && model.Password == "admin")
        {
            // Генерируем JWT токен
            var token = GenerateJwtToken(model.Email);

            // Возвращаем Bearer JWT токен
            return Ok(new { token });
        }

        // Если аутентификация не удалась, возвращаем Unauthorized
        return Unauthorized();
    }

    private string GenerateJwtToken(string username)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: new[] { new Claim(ClaimTypes.Name, username) },
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}



