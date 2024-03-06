using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TaskManager.Database;
using TaskManager.Database.Models;
using TaskManager.Schemas;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManager.Controllers
{
    [SwaggerTag("groups")]
    [Route("api/group")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class GroupsControllers : ControllerBase
    {
        private readonly TaskManagerContext _context;

        public GroupsControllers(TaskManagerContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "get-groups")]
        public async Task<ActionResult<List<Group>>> GetGroups([FromQuery] Guid? teamId)
        {
            ICollection<Group> groups;
            if (teamId != null)
            {
                var team = _context.Teams
                    .Include(t => t.Groups)
                        .ThenInclude(g => g.Users)
                    .Include(t => t.Groups)
                        .ThenInclude(g => g.Owner)
                    .FirstOrDefault(t => t.Id == teamId);
                if (team == null)
                {
                    return BadRequest(new JsonResult("Команда не найдена") { StatusCode = 401}); 
                }
                groups = team.Groups;
            } else {
                groups = await _context.Groups
                    .Include(g => g.Users)
                    .Include(g => g.Owner)
                    .ToListAsync(); 
            }

            return Ok(groups);
        }

        [HttpPost(Name = "create-group")]
        public async Task<ActionResult<Group>> CreateGroup([FromBody] CreateGroupScheme model)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == model.TeamId); 
            if (team == null)
            {
                return NotFound(new JsonResult("Команда не найдена"));
            }
            var groups = team.Groups;
            foreach ( var group in groups ) 
            {
                if (group.Name == model.Name)
                {
                    return NotFound(new JsonResult("Такая группа уже найдена")); 
                }
            }
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name); 
            if (user == null)
            {
                throw new Exception("Ты что то сделал очень не так"); 
            }

            Group groupCreate = new Group()
            {
                Name = model.Name,
                Role = model.Role,
                Owner = user, 
            };

            await _context.Groups.AddAsync(groupCreate);
            await _context.SaveChangesAsync(); 

            team.Groups.Add(groupCreate);

            _context.Update(team);
            await _context.SaveChangesAsync();
            
            return Ok(groups);
        }

        [HttpDelete("{id}", Name = "delete-group")]
        public async Task<ActionResult<Group>> DeleteGroup(Guid id)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(x => x.Id == id);
            if (group == null)
            {
                return NotFound(new JsonResult("Не нашли группа") { StatusCode = 404 });
            }
            if (group.Owner.Email != User.Identity.Name)
            {
                return new JsonResult("Доступ отказан") { StatusCode = 403 };
            }
            _context.Remove(group);
            await _context.SaveChangesAsync();

            return Ok(group);
        }
    }
}
