﻿using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NegotiationApp.Application.Interfaces;
using NegotiationApp.Domain.Entities;
using NegotiationApp.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace NegotiationApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;
        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
            _configuration = configuration;
        }

        public async Task<(User user, string error)> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Failed)
            {
                return (null!, "username or password is incorrect");
            }

            return (user, null!);
        }

        public async Task AddUserAsync(User user, string password)
        {
            if (await _userRepository.UsernameExistsAsync(user.Username))
            {
                throw new InvalidOperationException("Username already exists.");
            }

            user.SetPasswordHash(_passwordHasher.HashPassword(user, password));
           
            await _userRepository.AddAsync(user);
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    
    
}
