using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 

public class Comment
{
    [Key]
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;

    // Many-to-one relationship: Comment -> Task
    public TaskModel Task { get; set; } = new TaskModel();
}