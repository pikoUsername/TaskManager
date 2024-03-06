using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 

public class Comment
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Text { get; set; } = string.Empty;
    [Required]
    public DateTime CreatedAt { get; set; }

    // Many-to-one relationship: Comment -> Task
    [Required]
    public TaskModel Task { get; set; } = new TaskModel();

}