using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Database.Models; 

public class TaskModel
{
    [Key]
    [Required] public Guid Id { get; set; }
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public string Status { get; set; } = null!; 
    [Required]
    public DateTime StartedAt { get; set; }
    public DateTime EndsAt { get; set; }

    // Many-to-one relationships: Task -> User (Assigned_User, Created_By)
    public UserModel? AssignedUser { get; set; }
    [Required]
    public Project Project { get; set; }

    // Many-to-many relationship: Task <-> TaskTags
    [Required]
    public UserModel CreatedBy { get; set; }   
    public ICollection<TaskTag>? Tags { get; set; }
}