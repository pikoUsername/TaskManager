using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using Swashbuckle.AspNetCore.Annotations;
using TaskManager.Database;
using TaskManager.Database.Models;
using TaskManager.Schemas;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManager.Controllers
{
    [SwaggerTag("projects")]
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

            TaskType inWorkType = new TaskType() {
                Name = "В работе"
            };

            TaskType TodoWorkType = new TaskType()
            {
                Name = "К выполнению"
            };

            TaskType CompletedWorkType = new TaskType()
            {
                Name = "Завершено"
            };
            Project projectCreate = new Project() { 
                Description = model.Description,
                Name = model.Name,
                CreatedBy = user, 
            };

            projectCreate.TaskTypes.Add(inWorkType);
            projectCreate.TaskTypes.Add(CompletedWorkType);
            projectCreate.TaskTypes.Add(TodoWorkType); 

            var project = await _context.Projects.AddAsync(projectCreate);
            await _context.SaveChangesAsync();

            return Ok(); 
        }

        [HttpGet("/one", Name = "get-project")]
        [Authorize]
        public async Task<IActionResult> GetProject([FromQuery] ProjectSelecorScheme selector)
        {
            if (selector.Name == null && selector.Id == null)
            {
                return BadRequest("Не правильный формат");
            }
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Name == selector.Name || x.Id == selector.Id);
            if (project == null)
            {
                return NotFound("Проект не найден");
            } 

            return Ok(new JsonResult(project)); 
        }

        [HttpGet(Name = "get-projects-list")]
        [Authorize]
        public async Task<IActionResult> GetProjectsList([FromQuery] string? name)
        {
            List<Project> projects; 
            if (name == null)
            {
                projects = await _context.Projects.ToListAsync();
            }
            else
            {
                projects = await _context.Projects
                    .Where(u => EF.Functions.ToTsVector(u.Name).Matches(EF.Functions.ToTsQuery(name)))
                    .ToListAsync();
            }
            return Ok(projects);
        }

        [HttpPost("{id}/add/", Name = "add-user-to-project")]
        [Authorize]
        public async Task<IActionResult> AddUserToProject(Guid id, [FromBody] AddUserProjectScheme model)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);
            if (project == null)
            {
                return NotFound("Проект не найден");
            }
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            if (user == null)
            {
                return NotFound("Пользватель не найден");
            }
            project.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
