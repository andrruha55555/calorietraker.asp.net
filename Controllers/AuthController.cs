using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using calorietraker.Context;
using calorietraker.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace calorietraker.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _cfg;
        public AuthController(IConfiguration cfg) { _cfg = cfg; }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest();

            using var db = new AppDbContext();
            if (await db.Users.AnyAsync(u => u.Username == dto.Username || u.Email == dto.Email))
                return Conflict("User exists");

            var user = new User { Username = dto.Username, Email = dto.Email, PasswordHash = dto.Password };
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return StatusCode(201, user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginDto dto)
        {
            using var db = new AppDbContext();
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email && u.PasswordHash == dto.Password);
            if (user == null) return Unauthorized();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("username", user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]));
            var token = new JwtSecurityToken(
                issuer: _cfg["Jwt:Issuer"],
                audience: _cfg["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { token = jwt });
        }
    }

    public record RegisterDto(string Username, string Email, string Password);
    public record LoginDto(string Email, string Password);
}
