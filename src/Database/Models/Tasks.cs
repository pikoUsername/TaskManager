using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 

public class Task
{
    [Key]
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid Assigned_User { get; set; }
    public Guid CreatedBy { get; set; }
    public TaskStatus Status { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime EndsAt { get; set; }

    // Many-to-one relationships: Task -> User (Assigned_User, Created_By)
    public User AssignedUser { get; set; }

    // Many-to-many relationship: Task <-> TaskTags
    public ICollection<TaskTag> Tags { get; set; }
}