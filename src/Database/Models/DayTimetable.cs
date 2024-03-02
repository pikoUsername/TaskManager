using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 

public class DayTimetable
{
    [Key]
    public Guid Id { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public DayTimeTableTypes Type { get; set; } = DayTimeTableTypes.work;
    public DayTypes Day { get; set; } = DayTypes.Saturday; 
    public string Name { get; set; } = string.Empty;

    // One-to-many relationship: DayTimetable -> WorkVisits
    public ICollection<WorkVisit>? WorkVisits { get; set; }
}