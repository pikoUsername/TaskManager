using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 

public class Group
{
    [Key]
    public Guid Id { get; set; }
    public GroupRoles Role { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
    public User Owner { get; set; } = new User();
    // Many-to-one relationship: Group -> Team
    public Team Team { get; set; } = new Team();
}
