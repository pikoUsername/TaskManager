using System.ComponentModel.DataAnnotations;
using TaskManager.Database.Models;

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

    public class AddUserToTeam {
        [Required]
        public List<Guid> UserIds { get; set; }
        public string? Group { get; set; } = GroupRoles.employee; 
    }
}
