using System.ComponentModel.DataAnnotations;

namespace TaskManager.Database.Models; 

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string Full_Name { get; set; }
    public DateTime Created_At { get; set; }
    public DateTime Updated_At { get; set; }
    public string Email { get; set; }
    public string Hashed_Password { get; set; }
    public bool Blocked { get; set; }
    public byte[] Avatar { get; set; }
    public string Telegram { get; set; }
    public byte[] Banner { get; set; }
    public UserWorkTypes Work_Type { get; set; }

    // One-to-many relationship: User -> WorkVisits
    public ICollection<WorkVisit> WorkVisits { get; set; }
}
