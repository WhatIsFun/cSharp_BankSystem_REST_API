using cSharp_BankSystem_REST_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace cSharp_BankSystem_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }
        //[HttpPost("API-Login")]
        //public IActionResult APILogin(string email, string password)
        //{
        //    AuthenticateUser(login);
        //}
        [HttpPost("Login")]
        public IActionResult AuthenticateUser(userLogin login)
        {
            try
            {
                // Find the user by email
                User user = _context.Users.SingleOrDefault(u => u.Email == login.Email);

                if (user != null)
                {
                    if (VerifyPassword(login.Password, user.Password))
                    {
                        // Generate a JWT token
                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));
                        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Name, user.Name),
                            new Claim(ClaimTypes.Email, user.Email),
                        };

                        var token = new JwtSecurityToken(
                            issuer: "Mohammed",
                            audience: "Users",
                            claims: claims,
                            expires: DateTime.UtcNow.AddMinutes(20), // Token expiration time
                            signingCredentials: credentials
                        );
                        Log.Information($"new Login username: {user.Name}, {user.Email}");
                        return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                    }
                }
                // No matching user or incorrect password; return Unauthorized
                return Unauthorized("Invalid email or password.");
            }
            catch (Exception ex)
            {
                Log.Error("new error to login employee : " + login.Email);
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        // Implement a password verification method
        [HttpGet]
        public bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
        }
    }
}
