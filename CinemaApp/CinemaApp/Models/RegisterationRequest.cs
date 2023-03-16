﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Models
{
    internal class RegisterationRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public RegisterationRequest(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }
    }
}
