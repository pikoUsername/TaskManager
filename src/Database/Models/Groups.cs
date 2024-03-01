using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 

public class Group
{
    [Key]
    public Guid Id { get; set; }
    public GroupRoles Role { get; set; }

    public User Owner { get; set; }
    // Many-to-one relationship: Group -> Team
    public Team Team { get; set; }
}
