using Microsoft.AspNetCore.Mvc;
using TaskManager.Database.Models;
using TaskManager.Database;
using Microsoft.EntityFrameworkCore;
using TaskManager.Schemas; 

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManager.Controllers
{
    [Route("api/user/[controller]")]
    [ApiController]
    public class UserControllers : ControllerBase
    {
        private readonly TaskManagerContext _context;

        public UserControllers(TaskManagerContext context)
        {
            _context = context;
        }

        // GET: api/<ValuesController>
        [HttpGet(Name = "get-users")]
        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            return users; 
        }

        [HttpGet("{id}", Name = "get-user")]
        public async Task<User> GetUser(Guid id)
        {
            var user = await _context.Users.SingleOrDefaultAsync();

            return user; 
        }
    }
}
