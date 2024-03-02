using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TaskManager.Database;
using TaskManager.Database.Models;
using TaskManager.Schemas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManager.Controllers
{
    [SwaggerTag("tasks")]
    [Route("api/task/")]
    [ApiController]
    public class TaskControllers : ControllerBase
    {
        private readonly TaskManagerContext _context;

        public TaskControllers(TaskManagerContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "get-tasks")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<List<TaskModel>>> GetTasks([FromQuery] GetTasksScheme model)
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
            return Ok(tasks);
        }

        [HttpPost("{id}", Name = "assign-user-to-task")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<TaskModel>> AssignUserToTask(Guid id, [FromBody] AssignUserScheme model)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            if (task == null)
            {
                return NotFound(new JsonResult("Задача не найдена") { StatusCode = 404 });
            }

            var selectedUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            if (selectedUser == null)
            {
                return NotFound(new JsonResult("Пользователь не найден") { StatusCode = 404 });
            }
            task.AssignedUser = selectedUser;
            _context.Update(task);
            await _context.SaveChangesAsync();

            return Ok(task);
        }

        [HttpPatch("{id}", Name = "update-task")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<TaskModel>> UpdateTask(Guid id, [FromBody] UpdateTaskScheme model)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            if (task == null)
            {
                return NotFound(new JsonResult("Задача не найдена") { StatusCode = 404 });
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

            return Ok(task);
        }

        [HttpDelete("{id}", Name = "delete-task")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<TaskModel>> DeleteTask(Guid id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            if (task == null)
            {
                return NotFound(new JsonResult("Задача не найдена") { StatusCode = 404 });
            }
            _context.Remove(task);
            await _context.SaveChangesAsync();

            return Ok(task);
        }
    }
}
