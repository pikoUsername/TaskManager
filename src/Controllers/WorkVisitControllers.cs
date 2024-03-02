using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TaskManager.Database;
using TaskManager.Database.Models;
using TaskManager.Schemas;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace TaskManager.Controllers
{
    [Route("api/visits/[controller]")]
    [ApiController]
    [SwaggerTag("work-visits")]
    public class WorkVisitControllers : ControllerBase
    {
        private readonly TaskManagerContext _context;
        private readonly ILogger<WorkVisitControllers> _logger;
        private readonly IConfiguration _configuration;

        public WorkVisitControllers(
            TaskManagerContext context,
            IConfiguration configuration,
            ILogger<WorkVisitControllers> logger
        )
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost(Name = "register-work-visit")]
        public async Task<ActionResult<WorkVisit>> RegisterWorkVisit([FromBody] WorkVisitScheme model)
        {
            var worker = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.UserId);
            if (worker == null)
            {
                return NotFound(new JsonResult("Пользователь не найден") { StatusCode = 404 });
            }

            var dayName = DateTime.Now.DayOfWeek.ToString();
            var day = await _context.DayTimetables.FirstOrDefaultAsync(d => d.Day.ToString() == dayName);
            if (day == null)
            {
                return BadRequest(new JsonResult($"{dayName} отсутствует в базе данных") { StatusCode = 400 });
            }

            _logger.LogInformation($"Рабочий пришел на работу в {model.VisitedAt}. Рабочий: {worker}");

            WorkVisit visit = new WorkVisit()
            {
                User = worker,
                VisitedAt = DateTime.UtcNow,
                DayTimetable = day,
            };

            _context.WorkVisits.Add(visit);
            await _context.SaveChangesAsync();

            return Ok(visit);
        }
    }
}
