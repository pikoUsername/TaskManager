using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Database;
using TaskManager.Database.Models;
using TaskManager.Schemas;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManager.Controllers
{
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
        [Authorize]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentScheme model)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == model.TaskId); 
            if (task == null)
            {
                return NotFound("Задача не найдена"); 
            }

            var comment = new Comment()
            {
                Task = task,
                Text = model.Text,
            };
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(new JsonResult(comment)); 
        }

        [HttpGet(Name = "get-comments")]
        [Authorize]
        public async Task<IActionResult> GetComments([FromBody] GetCommentsScheme model)
        {
            var comments = await _context.Comments
                .Where(x => x.Task.Id == model.TaskId)
                .OrderBy(x => x.CreatedAt)
                .Skip(model.Start)
                .Take(model.End)
                .ToListAsync();

            return Ok(new JsonResult(comments)); 
        }

        [HttpDelete("{id}", Name = "delete-comment")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null)
            {
                return NotFound("Задача не найдена");
            }
            _context.Remove(comment);
            await _context.SaveChangesAsync(); 

            return Ok(new JsonResult(comment));
        }
    }
}
