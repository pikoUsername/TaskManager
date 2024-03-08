using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace TaskManager.Database.Models; 

public class Project
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty; 
    [Required] 
    public string Description { get; set; } = string.Empty;
    public FileModel? Icon { get; set; }

    public ICollection<TaskType>? TaskTypes { get; set; }
    // Many-to-one relationship: Project -> Team
    public Team? Team { get; set; }
    public ICollection<UserModel> Users { get; set; }
    [Required]
    public UserModel CreatedBy { get; set; } 
    public DateTime CreatedAt { get; set; } 

    public Project()
    {
        Users = new List<UserModel>();
    }
}