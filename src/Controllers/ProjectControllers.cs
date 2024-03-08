using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TaskManager.Database;
using TaskManager.Database.Models;
using TaskManager.Schemas;

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

        [HttpPost(Name = "create-project")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Project>> CreateProject([FromBody] CreateProjectScheme model)
        {
            var email = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email );
            if (user == null)
            {
                return NotFound(new JsonResult("Пользователь не найден") { StatusCode = 404 });
            }

            TaskType inWorkType = new TaskType() { Name = "В работе" };
            TaskType todoWorkType = new TaskType() { Name = "К выполнению" };
            TaskType completedWorkType = new TaskType() { Name = "Завершено" };

            Project projectCreate = new Project()
            {
                Description = model.Description,
                Name = model.Name,
                CreatedBy = user,
                TaskTypes = new List<TaskType>(), 
                CreatedAt = DateTime.UtcNow 
            };

            projectCreate.TaskTypes.Add(inWorkType);
            projectCreate.TaskTypes.Add(completedWorkType);
            projectCreate.TaskTypes.Add(todoWorkType);

            await _context.Projects.AddAsync(projectCreate);
            await _context.SaveChangesAsync();

            return Ok(projectCreate);
        }

        [HttpGet("one", Name = "get-project")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Project>> GetProject([FromQuery] ProjectSelecorScheme selector)
        {
            if (selector.Name == null && selector.Id == null)
            {
                return BadRequest(new JsonResult("Неправильный формат") { StatusCode = 400 });
            }

            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Name == selector.Name || x.Id == selector.Id);
            if (project == null)
            {
                return NotFound(new JsonResult("Проект не найден") { StatusCode = 404 });
            }

            return Ok(project);
        }

        [HttpGet(Name = "get-projects-list")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<List<Project>>> GetProjectsList([FromQuery] string? name)
        {
            List<Project> projects;
            if (string.IsNullOrEmpty(name))
            {
                projects = await _context.Projects
                    .Include(p => p.Users)
                    .Include(p => p.TaskTypes)
                    .Include(p => p.Team)
                    .Include(p => p.CreatedBy)
                    .Include(p => p.Icon)
                    .ToListAsync();
            }
            else
            {
                projects = await _context.Projects
                    .Where(u => EF.Functions.ToTsVector(u.Name).Matches(EF.Functions.ToTsQuery(name)))
                    .Include(p => p.Users)
                    .Include(p => p.TaskTypes)
                    .Include(p => p.Team)
                    .Include(p => p.CreatedBy)
                    .Include(p => p.Icon)
                    .ToListAsync();
            }

            return Ok(projects);
        }

        [HttpPost("{id}/add/", Name = "add-user-to-project")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Project>> AddUserToProject(Guid id, [FromBody] AddUserProjectScheme model)
        {
            var project = await _context.Projects
                .Include(p => p.Users)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (project == null)
            {
                return NotFound(new JsonResult("Проект не найден") { StatusCode = 401 });
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            if (user == null)
            {
                return NotFound(new JsonResult("Пользователь не найден") { StatusCode = 401 });
            }

            project.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(project);
        }

        [HttpPatch("{id}", Name = "update-project")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Project>> UpdateProject(Guid id, [FromBody] UpdateProjectSchema model)
        {
            var project = await _context.Projects
                .Include(p => p.Users)
                .Include(p => p.TaskTypes)
                .Include(p => p.Team)
                .Include(p => p.CreatedBy)
                .Include(p => p.Icon)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (project == null)
                return NotFound(new JsonResult("Проект не найден") { StatusCode = 401 });
            if (!string.IsNullOrEmpty(model.Name))
                project.Name = model.Name;
            if (!string.IsNullOrEmpty(model.Description))
                project.Description = model.Description;
            if (model.IconId != null)
            {
                var foundIcon = await _context.FileModels.FirstOrDefaultAsync(
                    x => x.Id == model.IconId);
                if (foundIcon == null)
                    return NotFound(new JsonResult("Файл не найден") { StatusCode = 401 }); 

                project.Icon = foundIcon;
            }

            _context.Update(project);
            await _context.SaveChangesAsync();

            return Ok(project); 
        }
    }
}
