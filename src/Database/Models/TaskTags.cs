using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 

public class TaskTag
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }

    // Many-to-many relationship: TaskTag <-> Task
    public ICollection<TaskModel> Tasks { get; set; }
}
