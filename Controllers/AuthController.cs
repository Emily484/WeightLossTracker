using Microsoft.AspNetCore.Mvc;
using WeightLossTracker.Helpers;

namespace WeightLossTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel user)
        {
            if (user.Username == "test" && user.Password == "password")
            {
                var secretKey = _configuration["JwtConfig:Secret"];
                var token = secretKey != null ? JwtTokenGenerator.GenerateToken(user.Username, secretKey) : null;
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }
    }

    public class UserLoginModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}