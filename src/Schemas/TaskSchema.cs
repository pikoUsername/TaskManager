using System.ComponentModel.DataAnnotations;
using TaskManager.Database.Models;

namespace TaskManager.Schemas
{
    public class GetTasksScheme
    {
        public Guid? ProjectId {  get; set; }
        public Guid? UserId { get; set; }
        public Guid? TeamId { get; set; }
        public bool UserTasks { get; set; } = false; 
    }

    public class AssignUserScheme
    {
        [Required]
        public Guid UserId { get; set; }
    }

    public class UpdateTaskScheme
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TaskStatus {  get; set; } = string.Empty;
    }

    public class CreateTaskSchema {
        [Required]
        public Guid ProjectId { get; set; }
        public Guid? AssignToUserId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Status { get; set; }
        public DateTime? StartsAt {  get; set; }
        public DateTime? EndsAt { get; set; }
    }
}
