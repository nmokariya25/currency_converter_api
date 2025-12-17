using CurrencyConverter.Application.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using CurrencyConverter.Application.Models;
using LoginRequest = CurrencyConverter.Application.Models.LoginRequest;

namespace CurrencyConverter.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {   
            var users = new Dictionary<string, string>
            {
                { "admin", "Admin" },
                { "user", "User" }
            };

            if (!users.ContainsKey(request.Username))
                return Unauthorized("Invalid username");

            // Generate token
            var token = _tokenService.GenerateToken(request.Username, users[request.Username]);

            return Ok(new { token });
        }
    }
}
