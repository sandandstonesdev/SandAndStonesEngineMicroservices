﻿namespace ApplicationService.DTO
{
    public class LoginDTO
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool Remember { get; set; } = false;
    }
}
