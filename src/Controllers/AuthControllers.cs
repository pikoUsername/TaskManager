using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using TaskManager.Schemas;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Database;
using Microsoft.EntityFrameworkCore;
using TaskManager.Database.Models;
using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.Annotations;

namespace TaskManager.Controllers;

[SwaggerTag("auth")]
[Route("api/auth/")]
[ApiController]
public class RegisterUserController : ControllerBase
{
    private readonly TaskManagerContext _context;
    private readonly IPasswordHasher<User> _passwordHasherService; 

    public RegisterUserController(
        TaskManagerContext context,
        IPasswordHasher<User> passwordHasherService
    )
    {
        _context = context;
        _passwordHasherService = passwordHasherService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserSchema model)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email || x.FullName == model.FullName);
        if (user != null) {
            return NotFound(new JsonResult("Такой пользватель уже существуют"));
        }
        User userCreate = new User()
        {
            FullName = model.FullName,
            Email = model.Email,
        };
        userCreate.HashedPassword = _passwordHasherService.HashPassword(userCreate, model.Password); 

        await _context.Users.AddAsync(userCreate);
        await _context.SaveChangesAsync(); 

        return new JsonResult(userCreate);
    }
}

[SwaggerTag("auth")]
[Route("api/auth/")]
[ApiController]
public class LoginUserController : ControllerBase
{
    private readonly TaskManagerContext _context;
    private readonly IConfiguration _configuration; 

    public LoginUserController(TaskManagerContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration; 
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserSchema model)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
        if (user == null)
        {
            return Unauthorized(); 
        }
        // Генерируем JWT токен
        var token = GenerateJwtToken(model.Email);

        // Возвращаем Bearer JWT токен
        return Ok(new { token });
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



