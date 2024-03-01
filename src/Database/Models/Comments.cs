using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 

public class Comment
{
    [Key]
    public Guid Id { get; set; }
    public string Text { get; set; }
    public Guid TaskId { get; set; }

    // Many-to-one relationship: Comment -> Task
    public TaskModel Task { get; set; }
}