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
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace TaskManager.Controllers
{
    [SwaggerTag("auth")]
    [Route("api/auth/")]
    [ApiController]
    public class RegisterUserController : ControllerBase
    {
        private readonly TaskManagerContext _context;
        private readonly IPasswordHasher<UserModel> _passwordHasherService;
        private readonly IConfiguration _configuration;

        public RegisterUserController(
            TaskManagerContext context,
            IPasswordHasher<UserModel> passwordHasherService,
            IConfiguration configuration
        )
        {
            _context = context;
            _passwordHasherService = passwordHasherService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserModel>> RegisterUser([FromBody] RegisterUserSchema model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email || x.FullName == model.FullName);
            if (user != null)
            {
                return BadRequest(new JsonResult("Такой пользователь уже существует"));
            }

            var userCreate = new UserModel()
            {
                FullName = model.FullName,
                Email = model.Email,
            };
            userCreate.HashedPassword = _passwordHasherService.HashPassword(userCreate, model.Password);

            await _context.Users.AddAsync(userCreate);
            await _context.SaveChangesAsync();

            return Ok(userCreate);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseSchema>> Login([FromBody] LoginUserSchema model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
            if (user == null)
            {
                return BadRequest(new JsonResult("Такой пользватель не существует") { StatusCode = 401});
            }

            // Генерируем JWT токен
            var token = GenerateJwtToken(model.Email);
            var expiresIn = _configuration["Jwt:ExpireMinutes"];
            if (expiresIn == null)
            {
                throw new Exception("Jwt:ExpireMinutes is empty"); 
            }

            // Возвращаем Bearer JWT токен
            return Ok(new LoginResponseSchema { AccessToken = token, ExpiresIn = expiresIn });
        }

        private string GenerateJwtToken(string username)
        {
            var secretKey = _configuration["Jwt:SecretKey"]; 
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new Exception("Jwt:SecretKey is empty"); 
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
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
}
