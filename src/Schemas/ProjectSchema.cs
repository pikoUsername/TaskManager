using System.ComponentModel.DataAnnotations;

namespace TaskManager.Schemas
{
    public class CreateProjectScheme
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty; 
    }

    public class ProjectSelecorScheme { 
        public string? Name { get; set; } 
        public Guid? Id { get; set; }
    }

    public class AddUserProjectScheme
    {
        [Required]
        public Guid UserId { get; set; }
    }
    
    public class UpdateProjectSchema
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid? IconId { get; set; }
        public Guid? OwnerId { get; set; }
    }
}
