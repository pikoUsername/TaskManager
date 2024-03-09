using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Database.Models; 


public class Team
{
    [Key]
    [Required] public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } 
    [Required]
    public bool Deleted { get; set; } = false; 
    public FileModel? Avatar { get; set; }
    [Required]
    public UserModel CreatedBy { get; set; } 
    // One-to-many relationship: Team -> Groups
    public ICollection<Group> Groups { get; set; }
    public ICollection<DayTimetable> DayTimetables { get; set; } = new List<DayTimetable>();

    public int MembersCount
    {
        get
        {
            return Groups.Count();
        }
    }
}
