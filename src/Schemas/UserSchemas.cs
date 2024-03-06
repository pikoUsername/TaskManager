using TaskManager.Database.Models;

namespace TaskManager.Schemas
{
    public class RegisterUserSchema
    {
        public string FullName { get; set; } = string.Empty; 
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginUserSchema
    {
        public string Email { get; set; } = string.Empty;
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
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; }
        public bool Blocked { get; set; } = false;
        public FileModel? Avatar { get; set; }
        public string WorkType { get; set; }
    } 
}
