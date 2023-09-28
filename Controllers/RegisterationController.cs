using cSharp_BankSystem_REST_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace cSharp_BankSystem_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterationController : ControllerBase
    {
        public static ApplicationDbContext _context;

        public RegisterationController(ApplicationDbContext DB)
        {
            _context = DB;
        }
        [HttpPost("Register")]
        public void Register(string name, string email, string password)
        {
            if (!IsValidEmail(email))
            {
                Console.WriteLine("Invalid email address.");
            }

            if (!IsValidPassword(password))
            {
                Console.WriteLine("Invalid password. Password must meet certain requirements.\nUppercase and Lowercase Letters\nDigits\nSpecial Characters (Minimum Length 8)");
                return;
            }

            string hashedPassword = HashPassword(password); //hashing the password 

            // If email and password are valid, insert data into the database
            InsertUserRegistrationData(name, email, hashedPassword);

            Console.WriteLine("User registration successful.");
        }

        private static bool IsValidEmail(string email)
        {
            string pattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$"; //Regular expression for email validation
            return Regex.IsMatch(email, pattern);
        }

        private static bool IsValidPassword(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$"; //Uppercase and Lowercase Letters, Digits, and Special Characters (Minimum Length 8):

            Regex regex = new Regex(pattern);
            return regex.IsMatch(password); // Return true if password meets your requirements
        }

        // Insert user registration data into the database
        
        private static void InsertUserRegistrationData(string name, string email, string password)
        {
            try
            {
                var _context = new ApplicationDbContext();
                var usr1 = new User { Name = name, Email = email, Password = password };
                _context.Add(usr1);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        private static string HashPassword(string password)
        {
            // BCrypt to hash the password
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
