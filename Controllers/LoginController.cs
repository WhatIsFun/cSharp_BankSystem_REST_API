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
        public void AuthenticateUser(string email, string password)
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
                        Console.WriteLine(user.Name, email);
                    }
                }
                // No matching user or incorrect password; return Unauthorized
                 Console.WriteLine("Error");
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine(ex.Message);
                Console.WriteLine("An error occurred while processing your request.");
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
