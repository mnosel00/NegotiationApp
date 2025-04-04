﻿using Microsoft.EntityFrameworkCore;
using NegotiationApp.Domain.Entities;
using NegotiationApp.Domain.Interfaces;
using NegotiationApp.Infrastructure.Data;

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

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Username == username && x.PasswordHash == password);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Username == username);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(x => x.Username == username);
        }
    }
}
