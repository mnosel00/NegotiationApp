using NegotiationApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegotiationApp.Application.DTOs
{
    public class RegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        public RegisterDto(string username, string password, Role role)
        {
            Username = username;
            Password = password;
            Role = role;
        }
    }
}
