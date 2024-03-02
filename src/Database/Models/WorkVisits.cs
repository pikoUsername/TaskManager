using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 

public class WorkVisit
{
    [Key]
    public Guid Id { get; set; }
    public DateTime VisitedAt { get; set; }
    public DateTime EndedAt { get; set; }

    // Many-to-one relationships: WorkVisit -> User, DayTimetable
    public UserModel User { get; set; } = new UserModel();
    public DayTimetable DayTimetable { get; set; } = new DayTimetable();    
}