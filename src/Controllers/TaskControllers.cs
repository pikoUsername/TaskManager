using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Database;
using TaskManager.Database.Models;
using TaskManager.Schemas;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManager.Controllers
{
    [Route("api/task/")]
    [ApiController]
    public class TaskControllers : ControllerBase
    {
        private readonly TaskManagerContext _context;

        public TaskControllers(TaskManagerContext context, IConfiguration configuration)
        {
            _context = context;
        }

        [HttpPost(Name = "get-tasks")]
        [Authorize]
        public async Task<IActionResult> GetTasks([FromQuery] GetTasksScheme model)
        {
            List<TaskModel> tasks;
            if (model.UserTasks)
            {
                var email = User.Identity.Name;
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

                tasks = await _context.Tasks
                    .Where(x => x.AssignedUser.Id == user.Id)
                    .ToListAsync();
            }
            else
            {
                tasks = await _context.Tasks
                    .Where(
                        x => x.AssignedUser.Id == model.UserId ||
                        x.Project.Id == model.ProjectId
                    )
                    .ToListAsync();
            } 
            return Ok(new JsonResult(tasks)); 
        }

        [HttpPost("{id}", Name = "assign-user-to-task")]
        [Authorize]
        public async Task<IActionResult> AssignUserToTask(Guid id, [FromBody] AssignUserScheme model)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id); 
            if (task == null)
            {
                return NotFound("Задача не найдена");
            }

            var selectedUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            if (selectedUser == null)
            {
                return NotFound("Пользватель не найден");
            }
            task.AssignedUser = selectedUser; 
            _context.Update(task);
            await _context.SaveChangesAsync(); 

            return Ok(new JsonResult(task)); 
        }

        [HttpPatch("{id}", Name = "update-task")]
        [Authorize]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskScheme model)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            if (task == null)
            {
                return NotFound("Задача не найдена");
            }

            if (model.Title != null)
            {
                task.Title = model.Title; 
            } 
            if (model.Description != null)
            {
                task.Description = model.Description;
            }

            _context.Update(task);
            await _context.SaveChangesAsync();

            return Ok(new JsonResult(task)); 
        }

        [HttpDelete("{id}", Name = "delete-task")]
        [Authorize]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            if (task == null)
            {
                return NotFound("Задача не найдена");
            }
            _context.Remove(task);
            await _context.SaveChangesAsync();

            return Ok(new JsonResult(task)); 
        }
    }
}
