using NegotiationApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegotiationApp.Application.Interfaces
{
    public interface IUserService
    {
        Task AddUserAsync(User user, string password);
        Task<User> AuthenticateAsync(string username, string password);
        string GenerateJwtToken(User user);
    }
}
