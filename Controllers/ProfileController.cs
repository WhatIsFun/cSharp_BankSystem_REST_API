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
        public List<Account> userAccounts = new List<Account>();
        [HttpPost("CreatAccount")]
        public void createAccount(User authenticatedUser, decimal initialBalance)
        {

            decimal balance = initialBalance;
            int UserID = authenticatedUser.User_Id;
            string AccountHolderName = authenticatedUser.Name;
            insertAccount(balance, UserID, AccountHolderName);

        }
        public static void insertAccount(decimal balance, int UserID, string AccountHolderName)
        {
            try
            {
                using (var _context = new ApplicationDbContext())
                {
                    var acc = new Account { User_Id = UserID, HolderName = AccountHolderName, Balance = balance };
                    _context.Add(acc);
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        [HttpGet("getUserAccount")]
        public static List<Account> GetUserAccounts(int userId)
        {
            using (var _context = new ApplicationDbContext())
            {
                List<Account> accounts = _context.Accounts
                    .Where(a => a.User_Id == userId)
                    .ToList();

                return accounts;
            }
        }
        [HttpDelete("deleteUserAccount")]
        public void deleteAccountServer(int accountIdToDelete)
        {
            try
            {
                using (var _context = new ApplicationDbContext())
                {
                    var accountToDelete = _context.Accounts.Find(accountIdToDelete);

                    if (accountToDelete != null)
                    {
                        _context.Accounts.Remove(accountToDelete);
                        _context.SaveChanges();

                        Console.WriteLine($"Account with ID {accountIdToDelete} deleted successfully.\nVisit nearest ATM to withdraw your balance");
                    }
                    else
                    {
                        Console.WriteLine($"Account with ID {accountIdToDelete} not found.");
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        [HttpDelete("deleteUser")]
        public void deleteUserServer(int userIdToDelete)
        {
            try
            {
                using (var _context = new ApplicationDbContext())
                {
                    var userDelete = _context.Users.Find(userIdToDelete);

                    if (userDelete != null)
                    {
                        _context.Users.Remove(userDelete);
                        _context.SaveChanges();

                        Console.WriteLine($"User with ID {userIdToDelete} deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"User with ID {userIdToDelete} not found.");
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private static bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
        }
    }
}
