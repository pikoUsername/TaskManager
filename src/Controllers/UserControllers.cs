using Microsoft.AspNetCore.Mvc;
using TaskManager.Database.Models;
using TaskManager.Database;
using Microsoft.EntityFrameworkCore;
using TaskManager.Schemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManager.Controllers
{
    [SwaggerTag("users")]
    [Route("api/user/")]
    [ApiController]
    public class UserControllers : ControllerBase
    {
        private readonly TaskManagerContext _context;
        private readonly IPasswordHasher<UserModel> _passwordHasherService;

        public UserControllers(
            TaskManagerContext context,
            IPasswordHasher<UserModel> passwordHasherService
        )
        {
            _context = context;
            _passwordHasherService = passwordHasherService;
        }

        // GET: api/<ValuesController>
        [HttpGet(Name = "get-users")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IEnumerable<UserModel>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }

        [HttpGet("{id}", Name = "get-user")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(
                x => x.Id == id);
            if (user == null)
            {
                return NotFound(new JsonResult("Not found") { StatusCode = 400 });
            }

            return Ok(user);
        }

        [HttpPatch("{id}", Name = "update-user")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserScheme model)
        {
            var user = await _context.Users.SingleOrDefaultAsync(
                x => x.Id == id);
            if (user == null)
            {
                return NotFound(new JsonResult("Not found") { StatusCode = 400 });
            }

            if (!string.IsNullOrEmpty(model.FullName))
            {
                user.FullName = model.FullName;
            }
            if (!string.IsNullOrEmpty(model.Email))
            {
                user.Email = model.Email;
            }
            if (!string.IsNullOrEmpty(model.Password))
            {
                user.HashedPassword = _passwordHasherService.HashPassword(user, model.Password);
            }

            _context.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new JsonResult(user));
        }

        [HttpGet("me", Name = "get-me")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<UserModel>> GetMe()
        {
            var user = await _context.Users.SingleOrDefaultAsync(
                x => x.Email == User.Identity.Name);
            if (user == null)
            {
                return NotFound(new JsonResult("Not found") { StatusCode = 400});
            }

            return Ok(new JsonResult(user));
        }
    }
} 
