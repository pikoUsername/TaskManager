using System.ComponentModel.DataAnnotations;

namespace TaskManager.Schemas
{
    public class LoginResponseSchema
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string ExpiresIn { get; set; }
    }
}
