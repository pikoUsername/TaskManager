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
    [SwaggerTag("team")]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamControllers : ControllerBase
    {
        private readonly TaskManagerContext _context;

        public TeamControllers(TaskManagerContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "get-teams-all")]
        [Authorize]
        public async Task<IActionResult> GetTeamsAll([FromQuery] GetTeamsSchema model)
        {
            List<Team> teams;
            if (model.UserId == null)
            {
                teams = await _context.Teams.ToListAsync();
            } else
            {
                teams = await _context.Groups
                    .Where(g => g.Users.Any(u => u.Id == model.UserId))
                    .Select(g => g.Team)
                    .ToListAsync();
            }

            return Ok(new JsonResult(teams));  
        }
        [HttpPost(Name = "create-team")]
        [Authorize]
        public async Task<IActionResult> CreateTeam([FromBody] CreateTeamSchema model)
        {
            Team team = new Team()
            {
                Name = model.Name,
            };
            Group defaultGroup = new Group()
            {
                Team = team, 
                Role = GroupRoles.employee,
                Users = new List<UserModel>(),
            };
            foreach (var userId in model.UserIds) {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    continue; 
                }
                defaultGroup.Users.Add(user);
            }
            var ownerUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name); 
            if (ownerUser == null)
            {
                return BadRequest("Not found"); 
            }

            Group ownerGroup = new Group()
            {
                Team = team,
                Role = GroupRoles.employee,
                Users = new List<UserModel>(),
            };
            ownerGroup.Users.Add(ownerUser); 

            team.Groups.Add(defaultGroup);
            team.Groups.Add(ownerGroup); 

            _context.Teams.Add(team); 
            await _context.SaveChangesAsync();

            return Ok(new JsonResult(team));
        }
    }
}
