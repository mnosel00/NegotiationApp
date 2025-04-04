using NegotiationApp.Domain.Entities;

namespace NegotiationApp.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> AuthenticateAsync(string username, string password);
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> UsernameExistsAsync(string username);
    }
}
