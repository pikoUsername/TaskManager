using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 

public class WorkVisit
{
    [Key]
    public Guid Id { get; set; }
    public DateTime Visited_At { get; set; }
    public DateTime Ended_At { get; set; }
    public Guid Day_Id { get; set; }
    public Guid User_Id { get; set; }

    // Many-to-one relationships: WorkVisit -> User, DayTimetable
    public User User { get; set; }
    public DayTimetable DayTimetable { get; set; }
}