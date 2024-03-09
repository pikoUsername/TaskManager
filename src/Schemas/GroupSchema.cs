using System.ComponentModel.DataAnnotations;
using TaskManager.Database.Models;

namespace TaskManager.Schemas
{
    public class CreateGroupScheme
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid TeamId  { get; set; }
        [Required]
        public string Role {  get; set; }
    }
}
