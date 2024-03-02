using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Database;
using TaskManager.Schemas;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManager.Controllers
{
    [Route("api/project/")]
    [ApiController]
    public class ProjectControllers : ControllerBase
    {
        private readonly TaskManagerContext _context;
        private readonly IConfiguration _configuration;

        public ProjectControllers(TaskManagerContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/<ValuesController>
        [HttpPost(Name = "create-project")]
        [Authorize]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectScheme model)
        {
            var email = User.Identity.Name;

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase)); 
            if (user == null)
            {
                return NotFound("User not found"); 
            }


            return Ok(); 
        }
    }
}
