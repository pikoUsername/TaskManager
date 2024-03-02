using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Database.Models; 


public class Team
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Deleted { get; set; } = false; 
    public FileModel? Avatar { get; set; }

    public UserModel CreatedBy { get; set; }
    // One-to-many relationship: Team -> Groups
    public ICollection<Group> Groups { get; set; }

    public Team()
    {
        Deleted = false; 
    }
}
