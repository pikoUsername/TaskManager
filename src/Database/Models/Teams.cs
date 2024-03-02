using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Database.Models; 


public class Team
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public bool Deleted { get; set; }
    public FileModel Avatar { get; set; }

    public UserModel CreatedBy { get; set; } = new UserModel();
    // One-to-many relationship: Team -> Groups
    public ICollection<Group> Groups { get; set; } = new List<Group>();

    public Team()
    {
        Deleted = false; 
    }
}
