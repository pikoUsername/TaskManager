using System.ComponentModel.DataAnnotations;

namespace TaskManager.Schemas
{
    public class GetTeamsSchema
    {
        public Guid? UserId { get; set; }
    }

    public class CreateTeamSchema {
        [Required]
        public string Name { get; set; }
        [Required]
        public List<Guid> UserIds { get; set; }
        [Required]
        public string Description { get; set; } 
    }

}
