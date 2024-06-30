using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeightLossTracker.Helpers;
using BCrypt.Net;

namespace WeightLossTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private string HashPassword(string password)
        {
            // Implement your password hashing logic here
            // For example, you can use a library like BCrypt to hash the password
            // Replace the return statement with your actual implementation
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserModel user)
        {
            // Check if the user already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == user.Username);
        
            if (existingUser != null)
            {
                return BadRequest(new { Message = "Username already exists." });
            }
        
            // Create a new user
            var newUser = new User
            {
                Username = user.Username,
                PasswordHash = HashPassword(user.Password) // Assume HashPassword is a method to hash passwords
            };
        
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
        
            // Optionally, generate a token for the new user
            var secretKey = _configuration["JwtConfig:Secret"];
            var token = secretKey != null ? JwtTokenGenerator.GenerateToken(user.Username, secretKey) : null;
        
            return Ok(new { Message = "User registered successfully", Token = token });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserModel user)
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

    public class UserModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}