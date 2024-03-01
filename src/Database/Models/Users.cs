using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Database.Models; 

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string FullName { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedAt { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public bool Blocked { get; set; } = false; 
    public FileModel? Avatar { get; set; }
    public string? Telegram { get; set; }
    public FileModel? Banner { get; set; }
    public UserWorkTypes WorkType { get; set; } = UserWorkTypes.office; 

    // One-to-many relationship: User -> WorkVisits
    public ICollection<WorkVisit> WorkVisits { get; set; }

    public User()
    {
        CreatedAt = DateTime.UtcNow;
    }
}
