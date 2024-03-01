using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 


public class Notification
{
    [Key]
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}