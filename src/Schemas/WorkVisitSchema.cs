using System.ComponentModel.DataAnnotations;

namespace TaskManager.Schemas
{
    public class WorkVisitScheme
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public DateTime VisitedAt { get; set; } 
    }
}
