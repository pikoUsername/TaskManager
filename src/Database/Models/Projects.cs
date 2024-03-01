using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 

public class Project
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid? TeamId { get; set; }

    public ICollection<TaskType> TaskTypes { get; set; }
    // Many-to-one relationship: Project -> Team
    public Team Team { get; set; }
}