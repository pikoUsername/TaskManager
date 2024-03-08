using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 

public class Group
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Role { get; set; }
    public ICollection<UserModel> Users { get; set; } = new List<UserModel>();
    public UserModel? Owner { get; set; }
}
