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
            var token = secretKey != null ? JwtTokenGenerator.GenerateToken(user.Username, secretKey!) : JwtTokenGenerator.GenerateToken(user.Username, string.Empty);
        
            return Ok(new { Message = "User registered successfully", Token = token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserModel user)
        {
            // Check if the user exists and the password is correct
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == user.Username);

            if (existingUser == null || !BCrypt.Net.BCrypt.Verify(user.Password, existingUser.PasswordHash))
            {
                return BadRequest(new { Message = "Username or password is incorrect." });
            }

            // Generate a token for the user
            var secretKey = _configuration["JwtConfig:Secret"];
            var token = secretKey != null ? JwtTokenGenerator.GenerateToken(user.Username, secretKey) : JwtTokenGenerator.GenerateToken(user.Username, string.Empty);

            return Ok(new { Token = token });
        }
    }

    public class UserModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}