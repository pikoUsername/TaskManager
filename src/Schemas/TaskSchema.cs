using TaskManager.Database.Models;

namespace TaskManager.Schemas
{
    public class GetTasksScheme
    {
        public Guid? ProjectId {  get; set; }
        public Guid UserId { get; set; }
        public Guid? TeamId { get; set; }
        public bool UserTasks { get; set; } = false; 
    }

    public class AssignUserScheme
    {
        public Guid UserId { get; set; }
    }

    public class UpdateTaskScheme
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class CreateTaskSchema {
        public Guid ProjectId { get; set; }
        public Guid? AssignToUserId { get; set; }
        public string Title { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; }
    }
}
