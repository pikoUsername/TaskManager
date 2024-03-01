using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 


public class Team
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid Creator_Id { get; set; }
    public bool Deleted { get; set; }

    // One-to-many relationship: Team -> Groups
    public ICollection<Group> Groups { get; set; }
}
