using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 

public class Group
{
    [Key]
    [Required] public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = null!; 
    [Required]
    public string Role { get; set; } = null!; 
    public ICollection<UserModel> Users { get; set; } = new List<UserModel>();
    public UserModel? Owner { get; set; }
}
