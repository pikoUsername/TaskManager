using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 

public class TaskType
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid Project_Id { get; set; } // Внешний ключ для связи с проектом

    // Многие типы задач могут быть связаны с одним проектом
    public Project Project { get; set; }
}