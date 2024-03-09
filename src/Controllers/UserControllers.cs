using Microsoft.AspNetCore.Mvc;
using TaskManager.Database.Models;
using TaskManager.Database;
using Microsoft.EntityFrameworkCore;
using TaskManager.Schemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.EntityFrameworkCore.Query;

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
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers([FromQuery] string? name)
        {
            // Костыль 
            IIncludableQueryable<UserModel, FileModel?> baseRequest; 
            if (name == null)
            {
                baseRequest = _context.Users
                    .Include(x => x.WorkVisits)
                    .Include(x => x.Avatar);
            }
            else
            {
                baseRequest = _context.Users
                    .Where(u => EF.Functions.ToTsVector(u.FullName).Matches(EF.Functions.ToTsQuery(name)))
                    .Include(x => x.WorkVisits)
                    .Include(x => x.Avatar);
            } 
            var users = await baseRequest
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}", Name = "get-user")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<UserModel>> GetUser(Guid id)
        {
            var user = await _context.Users
                .Include(x => x.WorkVisits)
                .Include(x => x.Avatar)
                .SingleOrDefaultAsync(
                x => x.Id == id);
            if (user == null)
            {
                return NotFound(new JsonResult("Not found") { StatusCode = 400 });
            }

            return Ok(user);
        }

        [HttpPatch("{id}", Name = "update-user")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<UserModel>> UpdateUser(Guid id, [FromBody] UpdateUserScheme model)
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

            return Ok(user);
        }

        [HttpGet("me", Name = "get-me")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<UserModel>> GetMe()
        {
            var user = await _context.Users
                .Include(x => x.WorkVisits)
                .Include(x => x.Avatar)
                .SingleOrDefaultAsync(
                x => x.Email == User.Identity.Name);
            if (user == null)
            {
                return NotFound(new JsonResult("Not found") { StatusCode = 400});
            }

            return Ok(user);
        }
    }
} 
