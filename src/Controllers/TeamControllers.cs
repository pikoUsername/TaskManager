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
    [Route("api/team/")]
    [ApiController]
    public class TeamControllers : ControllerBase
    {
        private readonly TaskManagerContext _context;

        public TeamControllers(TaskManagerContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "get-teams-all")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<List<Team>>> GetTeamsAll([FromQuery] GetTeamsSchema model)
        {
            var baseRequest = _context.Teams
                    .Include(x => x.Groups)
                        .ThenInclude(x => x.Users)
                            .ThenInclude(x => x.Avatar)
                    .Include(x => x.DayTimetables)
                    .Include(x => x.Avatar); 
            if (model.UserId == null)
            {
                var teams = await baseRequest
                    .ToListAsync();
                return Ok(teams); 
            } else
            {
                // TODO: make users specific get teams 
                var teams = await baseRequest
                    .ToListAsync();
                return Ok(teams); 
            }
        }

        [HttpPost(Name = "create-team")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Team>> CreateTeam([FromBody] CreateTeamSchema model)
        {
            var ownerUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
            if (ownerUser == null)
            {
                return NotFound(new JsonResult("Not found") { StatusCode = 400 });
            }

            var timeTables = DayTimetable.CreateDefaultTimeTable();

            _context.AddRange(timeTables);
            await _context.SaveChangesAsync(); 

            Team team = new Team()
            {
                Name = model.Name,
                Groups = new List<Group>(), 
                CreatedBy = ownerUser,
                Description = model.Description, 
                DayTimetables = timeTables, 
            };

            Group defaultGroup = new Group()
            {
                Name = "Сотрудники",
                Owner = ownerUser, 
                Role = GroupRoles.employee,
                Users = new List<UserModel>()
            };

            Group ownerGroup = new Group()
            {
                Name = "Руководители",
                Role = GroupRoles.employee,
                Owner = ownerUser,
                Users = new List<UserModel>()
            };
            foreach (var userId in model.UserIds)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    defaultGroup.Users.Add(user);
                }
            }
            ownerGroup.Users.Add(ownerUser); 
            await _context.Groups.AddAsync(defaultGroup);
            await _context.Groups.AddAsync(ownerGroup);

            await _context.SaveChangesAsync();

            team.Groups.Add(defaultGroup);
            team.Groups.Add(ownerGroup);


            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync(); 

            return Ok(team); 
        }

        [HttpPost("{id}/groups/adduser", Name = "add-user-to-team")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Team>> AddUserToTeam(Guid id, [FromBody] AddUserToTeam model)
        {
            var team = await _context.Teams
                .Include(x => x.Groups)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (team == null) {
                return NotFound(new JsonResult("Команда не найдена") { StatusCode = 401 });     
            }
            string groupName; 

            if (model.Group == null)
            {
                groupName = GroupRoles.employee; 
            } else
            {
                groupName = model.Group; 
            }
            var group = team.Groups.FirstOrDefault(x => x.Role == groupName);
            if (group == null)
            {
                throw new Exception("Ты нарушил инварианты бизнес логики!!"); 
            }

            foreach (var userId in model.UserIds)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (user == null)
                {
                    return NotFound(new JsonResult($"{userId} пользватель не найден!")); 
                }
                if (group.Users.Contains(user))
                {
                    continue;  // ignore 
                }
                group.Users.Add(user); 
            }
            _context.Update(group);
            _context.Update(team); 
            await _context.SaveChangesAsync();  

            return Ok(team);
        }

        [HttpGet("{id}", Name = "get-team")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Team>> GetTeam(Guid id)
        {
            var team = await _context.Teams
                    .Include(x => x.Groups)
                        .ThenInclude(x => x.Users)
                            .ThenInclude(x => x.Avatar)
                    .Include(x => x.DayTimetables)
                    .Include(x => x.Avatar) 
                    .FirstOrDefaultAsync(x => x.Id == id); 
            if (team == null)
            {
                return NotFound(new JsonResult("Нету команды") { StatusCode = 401 }); 
            }
            return Ok(team);
        }

        [HttpGet("{id}/attendance", Name = "get-team-attendance")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<List<AttendanceUserScheme>>> GetTeamAttendance(Guid id, string workType)
        {
            var team = await _context.Teams.FirstOrDefaultAsync();
            if (team == null)
                return NotFound(new JsonResult("Команда не найдена") { StatusCode = 401 });
            var workVisits = await _context.WorkVisits
                .Where(x => team.DayTimetables.Contains(x.DayTimetable))
                .ToListAsync(); 
            var users = await _context.Users
                .Where(x => x.WorkType == workType)
                .Include(x => x.Avatar)
                .ToListAsync();
            List<AttendanceUserScheme> resultUsers = new List<AttendanceUserScheme>();

            foreach (var user in users)
            {
                resultUsers.Add(
                    new AttendanceUserScheme()
                    {
                        FullName = user.FullName, 
                        Email = user.Email, 
                        Blocked = user.Blocked, 
                        Avatar = user.Avatar, 
                        WorkType = user.WorkType, 
                    }
                ); 
            }

            return Ok(resultUsers); 
        }
    }
}
