namespace TaskManager.Schemas
{
    public class RegisterUserSchema
    {
        public string FullName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class LoginUserSchema
    {
        public string Email { get; set; }
        public string Password { get; set; } 
    }
}
