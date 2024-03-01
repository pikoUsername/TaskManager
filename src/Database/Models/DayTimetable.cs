using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 

public class DayTimetable
{
    [Key]
    public Guid Id { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public DayTimeTableTypes Type { get; set; }
    public DayTypes Day { get; set; }
    public string Name { get; set; }

    // One-to-many relationship: DayTimetable -> WorkVisits
    public ICollection<WorkVisit> WorkVisits { get; set; }
}