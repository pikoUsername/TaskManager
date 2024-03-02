using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TaskManager.Database;
using TaskManager.Database.Models;
using TaskManager.Schemas;

namespace TaskManager.Controllers
{
    [Route("api/visits/[controller]")]
    [ApiController]
    [SwaggerTag("work-visits")]
    public class WorkVisitControllers : ControllerBase
    {
        private readonly TaskManagerContext _context;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration; 

        public WorkVisitControllers(
            TaskManagerContext context,
            IConfiguration configuration,
            ILogger logger 
        )
        {
            _context = context;
            _logger = logger;
            _configuration = configuration; 
        }


        [HttpPost(Name = "register-work-visit")]
        public async Task<IActionResult> RegisterWorkVisit([FromBody] WorkVisitScheme model)
        {
            var worker = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.UserId); 
            if (worker != null)
            {
                return NotFound("Пользватель не был найден"); 
            }

            var dayName = System.DateTime.Now.DayOfWeek.ToString();
            var day = await _context.DayTimetables.FirstOrDefaultAsync(d => d.Day.ToString() == dayName); 
            if (day != null)
            {
                throw new Exception($"{dayName} is not available in database, so get make it!!"); 
            }

            _logger.LogInformation($"Worker has come to work!! at: {model.VisitedAt}. Worker: {worker}");

            WorkVisit visit = new WorkVisit()
            {
                User = worker,
                VisitedAt = DateTime.UtcNow,
                DayTimetable = day,
            };

            _context.WorkVisits.Add(visit);
            await _context.SaveChangesAsync();

            return Ok(new JsonResult(visit)); 
        }
    }
}
