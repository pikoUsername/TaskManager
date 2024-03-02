﻿namespace TaskManager.Schemas
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
}
