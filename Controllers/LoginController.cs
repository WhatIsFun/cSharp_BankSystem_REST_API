using cSharp_BankSystem_REST_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("Login")]
        public IActionResult AuthenticateUser(string email, string password)
        {
            try
            {
                // Find the user by email
                User user = _context.Users.SingleOrDefault(u => u.Email == email);

                if (user != null)
                {
                    if (VerifyPassword(password, user.Password))
                    {
                        // Password is correct; return the user
                        return Ok(user);
                    }
                }
                // No matching user or incorrect password; return Unauthorized
                return Unauthorized();
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // Implement a password verification method
        public bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
        }
    }
}
