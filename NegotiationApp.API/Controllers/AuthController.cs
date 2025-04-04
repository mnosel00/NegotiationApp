using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NegotiationApp.Application.DTOs;
using NegotiationApp.Application.Interfaces;
using NegotiationApp.Domain.Entities;
using System.CodeDom.Compiler;

namespace NegotiationApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        { 
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var (user, error) = await _userService.AuthenticateAsync(loginDto.Username, loginDto.Password);


            if (user == null)
            {
                return BadRequest(new { message = error });
            }

            var token = _userService.GenerateJwtToken(user);
            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var user = new User(registerDto.Username, null);
                await _userService.AddUserAsync(user, registerDto.Password);
                return Ok();
            }
            catch (InvalidOperationException ex) when (ex.Message == "Username already exists.")
            {
                return BadRequest(new { message = "username already exists" });
            }
        }
    }
}
