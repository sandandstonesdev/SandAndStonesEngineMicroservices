﻿namespace SandAndStones.Api.DTO
{
    public class RegisterRequest
    {
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string ConfirmedPassword { get; set; } = "";
    }
}
