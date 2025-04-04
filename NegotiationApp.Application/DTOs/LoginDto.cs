using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegotiationApp.Application.DTOs
{
    public class LoginDto
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public LoginDto(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
