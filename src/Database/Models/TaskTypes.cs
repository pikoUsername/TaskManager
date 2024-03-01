using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 

public class TaskType
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }

    // Многие типы задач могут быть связаны с одним проектом
    public Project Project { get; set; }
}