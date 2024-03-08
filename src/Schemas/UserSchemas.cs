using System.ComponentModel.DataAnnotations;
using TaskManager.Database.Models;

namespace TaskManager.Schemas
{
    public class RegisterUserSchema
    {
        [Required]
        public string FullName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginUserSchema
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class UpdateUserScheme
    {
        public string? Email { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public string? FullName { get; set; } = string.Empty;
    }

    public class AttendanceUserScheme
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string FullName { get; set; }
        [Required]
        public bool Blocked { get; set; } = false;
        public FileModel? Avatar { get; set; }
        public string WorkType { get; set; }
    } 
}
