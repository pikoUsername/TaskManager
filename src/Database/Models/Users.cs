using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace TaskManager.Database.Models; 

public class UserModel
{
    [Key]
    [Required] public Guid Id { get; set; }

    [Required]
    public string FullName { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    public string? HashedPassword { get; set; }
    public bool Blocked { get; set; } = false; 
    public FileModel? Avatar { get; set; }
    public string? Telegram { get; set; }
    public FileModel? Banner { get; set; }
    [Required]
    public string WorkType { get; set; }  

    // One-to-many relationship: User -> WorkVisits
    public ICollection<WorkVisit> WorkVisits { get; set; } = new List<WorkVisit>();
}
