using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 

public class DayTimetable
{
    [Key]
    [Required] public Guid Id { get; set; }
    [Required]
    public DateTime StartsAt { get; set; }
    [Required]
    public DateTime EndsAt { get; set; }
    public string Type { get; set; } = DayTimeTableTypes.work;
    public string SubType { get; set; } = DayTimeTableSubTypes.work; 
    public DayTypes Day { get; set; } = DayTypes.Saturday;
    [Required]
    public string Name { get; set; } = string.Empty;

    // One-to-many relationship: DayTimetable -> WorkVisits
    //public ICollection<WorkVisit>? WorkVisits { get; set; }

    public static List<DayTimetable> CreateDefaultTimeTable()
    {
        List<DayTimetable> days = [];

        var workStart = new DateTime(2024, 1, 1, 9, 0, 0); 

        days.Add(new DayTimetable
        {
            Name = "Понидельник",
            StartsAt = workStart,  
            EndsAt = workStart.AddHours(4),
            Day = DayTypes.Monday,
            Type = DayTimeTableTypes.work,
            SubType = DayTimeTableSubTypes.work,
        });

        days.Add(new DayTimetable
        {
            Name = "Вторник",
            StartsAt = workStart,
            EndsAt = DateTime.UtcNow.AddHours(8),
            Day = DayTypes.Tuesday,
            SubType = DayTimeTableSubTypes.work,
            Type = DayTimeTableTypes.work,
        });


        days.Add(new DayTimetable
        {
            Name = "Среда",
            StartsAt = workStart,
            EndsAt = DateTime.UtcNow.AddHours(8),
            Day = DayTypes.Wednesday,
            SubType = DayTimeTableSubTypes.work,
            Type = DayTimeTableTypes.work,
        });


        days.Add(new DayTimetable
        {
            Name = "Четверг",
            StartsAt = workStart,
            EndsAt = DateTime.UtcNow.AddHours(8),
            SubType = DayTimeTableSubTypes.work,
            Day = DayTypes.Thursday,
            Type = DayTimeTableTypes.work,
        });


        days.Add(new DayTimetable
        {
            Name = "Пятница",
            StartsAt = workStart,
            SubType = DayTimeTableSubTypes.work,
            EndsAt = DateTime.UtcNow.AddHours(8),
            Day = DayTypes.Friday, 
            Type = DayTimeTableTypes.work,
        });


        days.Add(new DayTimetable
        {
            Name = "Суббота",
            StartsAt = workStart,
            EndsAt = DateTime.UtcNow.AddHours(8),
            Day = DayTypes.Saturday,
            Type = DayTimeTableTypes.weekend, 
            SubType = DayTimeTableSubTypes.work, 
        });


        days.Add(new DayTimetable
        {
            Name = "Воскресенье",
            StartsAt = workStart,
            EndsAt = DateTime.UtcNow.AddHours(8),
            Day = DayTypes.Wednesday,
            SubType = DayTimeTableSubTypes.work,
            Type = DayTimeTableTypes.weekend
        });

        days.Add(new DayTimetable
        {
            Name = "Общее",
            StartsAt = workStart,
            EndsAt = DateTime.UtcNow.AddHours(8),
            Day = DayTypes.All,
            SubType = DayTimeTableSubTypes.breakTime, 
            Type = DayTimeTableTypes.general
        });


        return days; 
    }
}