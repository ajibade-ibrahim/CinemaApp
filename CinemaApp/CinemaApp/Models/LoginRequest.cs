using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Models
{
    internal class LoginRequest
    {
        public LoginRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; set; }
        public string Password { get; set; }
    }
}
