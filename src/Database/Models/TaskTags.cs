using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 

public class TaskTag
{
    [Key]
    [Required] public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;

    // Many-to-many relationship: TaskTag <-> Task
    public ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();
}
