using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Database.Models; 


public class Team
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public bool Deleted { get; set; }
    public FileModel Avatar { get; set; }

    public User CreatedBy { get; set; }
    // One-to-many relationship: Team -> Groups
    public ICollection<Group> Groups { get; set; }

    public Team()
    {
        Deleted = false; 
    }
}
