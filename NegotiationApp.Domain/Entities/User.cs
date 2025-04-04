using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegotiationApp.Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public Role Role { get; private set; }

        public User(string username, string passwordHash, Role role)
        {
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
        }
        public void SetPasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
        }
    }

    public enum Role
    {
        Admin,
        User
    }
}
