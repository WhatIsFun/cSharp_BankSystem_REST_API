using cSharp_BankSystem_REST_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cSharp_BankSystem_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        public static ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext DB)
        {
            _context = DB;
        }

        [HttpPost("CreatAccount")]
        public IActionResult CreateAccount(User authenticatedUser, decimal initialBalance)
        {
            try
            {
                decimal balance = initialBalance;
                int UserID = authenticatedUser.User_Id;
                string AccountHolderName = authenticatedUser.Name;

                InsertAccount(balance, UserID, AccountHolderName);

                return Ok("Account created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        public static void InsertAccount(decimal balance, int UserID, string AccountHolderName)
        {
            try
            {
                var acc = new Account { User_Id = UserID, HolderName = AccountHolderName, Balance = balance };
                _context.Add(acc);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [HttpGet("getUserAccount")]
        public IActionResult GetUserAccounts(int userId)
        {
            try
            {
                var accounts = GetUserAccountsFromDatabase(userId);
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        public static List<Account> GetUserAccountsFromDatabase(int userId)
        {
            List<Account> accounts = _context.Accounts
                    .Where(a => a.User_Id == userId)
                    .ToList();

            return accounts;
        }

        [HttpDelete("deleteUserAccount")]
        public IActionResult DeleteAccountServer(int accountIdToDelete)
        {
            try
            {
                    var accountToDelete = _context.Accounts.Find(accountIdToDelete);

                if (accountToDelete != null)
                {
                    _context.Accounts.Remove(accountToDelete);
                    _context.SaveChanges();

                    return Ok($"Account with ID {accountIdToDelete} deleted successfully.\nVisit nearest ATM to withdraw your balance");
                }
                else
                {
                    return NotFound($"Account with ID {accountIdToDelete} not found.");
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("deleteUser")]
        public IActionResult DeleteUserServer(int userIdToDelete)
        {
            try
            {
                    var userDelete = _context.Users.Find(userIdToDelete);

                if (userDelete != null)
                {
                    _context.Users.Remove(userDelete);
                    _context.SaveChanges();

                    return Ok($"User with ID {userIdToDelete} deleted successfully.");
                }
                else
                {
                    return NotFound($"User with ID {userIdToDelete} not found.");
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        private static bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
        }
    }
}
