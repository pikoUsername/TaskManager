using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TaskManager.Database;
using TaskManager.Database.Models;
using TaskManager.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Controllers
{
    [SwaggerTag("comments")]
    [Route("api/comment/")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly TaskManagerContext _context;

        public CommentController(TaskManagerContext context)
        {
            _context = context;
        }

        [HttpPost(Name = "create-comment")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Comment>> CreateComment([FromBody] CreateCommentScheme model)
        {
            var task = await _context.Tasks
                .Include(x => x.Tags)
                .Include(x => x.Project)
                .Include(x => x.AssignedUser)
                .Include(x => x.CreatedBy)
                .FirstOrDefaultAsync(x => x.Id == model.TaskId);
            if (task == null)
            {
                return NotFound(new JsonResult("Задача не найдена") { StatusCode = 404 });
            }

            var comment = new Comment()
            {
                Task = task,
                Text = model.Text,
                CreatedAt = DateTime.UtcNow
            };
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }

        [HttpGet(Name = "get-comments")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<List<Comment>>> GetComments([FromQuery] GetCommentsScheme model)
        {
            var comments = await _context.Comments
                .Include(x => x.Task)
                .Where(x => x.Task.Id == model.TaskId)
                .OrderBy(x => x.CreatedAt)
                .Skip(model.Start)
                .Take(model.End)
                .ToListAsync();

            return Ok(comments);
        }

        [HttpDelete("{id}", Name = "delete-comment")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Comment>> DeleteComment(Guid id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null)
            {
                return NotFound(new JsonResult("Комментарий не найден") { StatusCode = 404 });
            }
            _context.Remove(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }

        [HttpPatch("{id}", Name = "update-comment")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Comment>> UpdateComment(Guid id, [FromBody] UpdateCommentSchema model)
        {
            var comment = await _context.Comments
                .Include(x => x.Task)
                    .ThenInclude(x => x.Project)
                .Include(x => x.Task)
                    .ThenInclude(x => x.AssignedUser)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null)
                return NotFound(new JsonResult("Коментарий не найден") { StatusCode = 401});
            comment.Text = model.Text;

            _context.Update(comment);
            await _context.SaveChangesAsync(); 

            return Ok(comment);
        }
    }
}
