using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Database.Models; 

public class TaskModel
{
    [Key]
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskStatus Status { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime EndsAt { get; set; }

    // Many-to-one relationships: Task -> User (Assigned_User, Created_By)
    public UserModel? AssignedUser { get; set; }
    public Project Project { get; set; } 

    // Many-to-many relationship: Task <-> TaskTags
    public UserModel CreatedBy { get; set; }   
    public ICollection<TaskTag>? Tags { get; set; }
}