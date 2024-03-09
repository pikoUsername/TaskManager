using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 


public class Notification
{
    [Key]
    [Required] public Guid Id { get; set; }
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty; 
}