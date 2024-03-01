using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 

public class Comment
{
    [Key]
    public Guid Id { get; set; }
    public string Text { get; set; }
    public Guid Task_Id { get; set; }

    // Many-to-one relationship: Comment -> Task
    public Task Task { get; set; }
}