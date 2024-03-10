using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TaskManager.Database;
using TaskManager.Database.Models;
using TaskManager.Schemas;

namespace TaskManager.Controllers
{
    [SwaggerTag("work-visits")]
    [Route("api/visit/")]
    [ApiController]
    public class WorkVisitControllers : ControllerBase
    {
        private readonly TaskManagerContext _context;
        private readonly ILogger<WorkVisitControllers> _logger;

        public WorkVisitControllers(
            TaskManagerContext context,
            ILogger<WorkVisitControllers> logger
        )
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost(Name = "register-work-visit")]
        public async Task<ActionResult<WorkVisit>> RegisterWorkVisit([FromBody] WorkVisitScheme model)
        {
            var worker = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.UserId);
            if (worker == null)
            {
                return NotFound(new JsonResult("Пользователь не найден") { StatusCode = 404 });
            }

            var dayName = DateTime.UtcNow.DayOfWeek; 
            var actualDayName = DayTypesService.FromSTDWeekDay(dayName);
            var day = await _context.DayTimetables.FirstOrDefaultAsync(d => d.Day == actualDayName);
            if (day == null)
            {
                return BadRequest(new JsonResult($"{dayName} отсутствует в базе данных") { StatusCode = 400 });
            }

            _logger.LogInformation($"Рабочий пришел на работу в {model.VisitedAt}. Рабочий: {worker}");

            WorkVisit visit = new WorkVisit()
            {
                VisitedAt = DateTime.UtcNow,
                DayTimetable = day,
            };

            worker.WorkVisits.Add(visit);

            _context.WorkVisits.Add(visit);
            await _context.SaveChangesAsync();

            return Ok(visit);
        }
    }
}
