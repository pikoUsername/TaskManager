using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 

public class Group
{
    [Key]
    public Guid Id { get; set; }
    public GroupRoles Role { get; set; }
    public ICollection<UserModel> Users { get; set; } = new List<UserModel>();
    public UserModel? Owner { get; set; } = new UserModel();
    // Many-to-one relationship: Group -> Team
    //public Team Team { get; set; } = new Team();
}
