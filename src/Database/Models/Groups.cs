namespace TaskManager.Database.Models; 

public class Group
{
    [Key]
    public Guid Id { get; set; }
    public Guid Owner_Id { get; set; }
    public Guid Team_Id { get; set; }
    public GroupRoles Role { get; set; }

    // Many-to-one relationship: Group -> Team
    public Team Team { get; set; }
}
