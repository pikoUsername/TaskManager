namespace TaskManager.Schemas
{
    public class GetTeamsSchema
    {
        public Guid? UserId { get; set; }
    }

    public class CreateTeamSchema { 
        public string Name { get; set; }
        public List<Guid> UserIds { get; set; }
    }

}
