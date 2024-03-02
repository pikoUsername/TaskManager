using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace TaskManager.Database.Models; 

public class User
{
    [Key]
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;


    public string Email { get; set; } = string.Empty;
    public string? HashedPassword { get; set; }
    public bool Blocked { get; set; } = false; 
    public FileModel? Avatar { get; set; }
    public string? Telegram { get; set; }
    public FileModel? Banner { get; set; }
    public UserWorkTypes WorkType { get; set; } = UserWorkTypes.office; 

    // One-to-many relationship: User -> WorkVisits
    public ICollection<WorkVisit> WorkVisits { get; set; } = new List<WorkVisit>();
}
