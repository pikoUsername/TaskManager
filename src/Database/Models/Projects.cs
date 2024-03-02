using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 

public class Project
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty; 
    public string Description { get; set; } = string.Empty;
    public FileModel? Icon { get; set; }

    public ICollection<TaskType>? TaskTypes { get; set; }
    // Many-to-one relationship: Project -> Team
    public Team Team { get; set; } = new Team();
    public User CreatedBy { get; set; } = new User();  
}