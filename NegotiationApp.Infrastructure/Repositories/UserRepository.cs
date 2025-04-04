using Microsoft.EntityFrameworkCore;
using NegotiationApp.Domain.Entities;
using NegotiationApp.Domain.Interfaces;
using NegotiationApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegotiationApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly NegotiationDbContext _context;
        public UserRepository(NegotiationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Username == username && x.PasswordHash == password);
        }
    }
}
