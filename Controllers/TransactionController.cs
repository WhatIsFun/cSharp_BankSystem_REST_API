using cSharp_BankSystem_REST_API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cSharp_BankSystem_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        public static ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext DB)
        {
            _context = DB;
        }
        [Authorize]
        [HttpPut("Deposit")]
        public IActionResult Deposit(List<Account> userAccounts, int sourceAccountId, decimal amount)
        {
            try
            {
                if (userAccounts.Count == 0)
                {
                    return BadRequest("Add an account first.");
                }
                else if (!userAccounts.Any(account => account.Account_Id == sourceAccountId))
                {
                    return BadRequest("Enter a valid account number.");
                }

                DateTime timestamp = DateTime.Now;
                string type = "Deposit";

                var dAccount = _context.Accounts.FirstOrDefault(a => a.Account_Id == sourceAccountId);

                if (dAccount != null)
                {
                    dAccount.Balance += amount;
                    _context.SaveChanges();

                    RecordTransaction(timestamp, type, amount, sourceAccountId, sourceAccountId, sourceAccountId);
                    return Ok("Transaction deposit added.");
                }
                else
                {
                    return NotFound("Invalid deposit.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [Authorize]
        [HttpPut("Withdraw")]
        public IActionResult Withdraw(List<Account> userAccounts, int sourceAccountId, decimal amount)
        {
            try
            {
                if (userAccounts.Count == 0)
                {
                    return BadRequest("Add an account first.");
                }
                else if (!userAccounts.Any(account => account.Account_Id == sourceAccountId))
                {
                    return BadRequest("Enter a valid account number.");
                }

                DateTime timestamp = DateTime.Now;
                string type = "Withdrawal";

                var dAccount = _context.Accounts.FirstOrDefault(a => a.Account_Id == sourceAccountId);

                if (dAccount != null)
                {
                    if (dAccount.Balance >= amount)
                    {
                        dAccount.Balance -= amount;
                        _context.SaveChanges();

                        RecordTransaction(timestamp, type, amount, sourceAccountId, sourceAccountId, sourceAccountId);
                        return Ok("Transaction Withdrawal added.");
                    }
                    else
                    {
                        return BadRequest("Insufficient funds for the withdrawal.");
                    }
                }
                else
                {
                    return NotFound("Invalid withdrawal.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [Authorize]
        [HttpPut("Transfer")]
        public IActionResult Transfer(List<Account> userAccounts, int sourceAccountId, int targetAccountId, decimal amount)
        {
            try
            {
                DateTime timestamp = DateTime.Now;
                var sourceAccount = _context.Accounts.FirstOrDefault(a => a.Account_Id == sourceAccountId);
                var targetAccount = _context.Accounts.FirstOrDefault(a => a.Account_Id == targetAccountId);

                if (sourceAccount == null || targetAccount == null)
                {
                    return NotFound("Source or target account not found.");
                }

                if (sourceAccount.Balance >= amount)
                {
                    string type = "Transfer";

                    sourceAccount.Balance -= amount;
                    targetAccount.Balance += amount;
                    _context.SaveChanges();

                    RecordTransaction(timestamp, type, amount, sourceAccountId, targetAccountId, sourceAccountId);
                    return Ok("Transaction added.");
                }
                else
                {
                    return BadRequest("Insufficient funds for the transfer.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        private void RecordTransaction(DateTime timestamp, string type, decimal amount, int sourceAccountId, int targetAccountId, int accountNum)
        {
            try
            {
                var transaction = new Transaction
                {
                    Timestamp = timestamp,
                    Type = type,
                    Amount = amount,
                    SorAccId = sourceAccountId,
                    TarAccId = targetAccountId,
                    User_Id = accountNum
                };

                _context.Transactions.Add(transaction);
                int rowsAffected = _context.SaveChanges();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Transaction recorded successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to record the transaction.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred while recording the transaction: " + e.Message);
            }
        }
        [Authorize]
        [HttpGet("Recording")]
        public IActionResult ViewTransactionHistory(User authenticatedUser, int viewAccId, DateTime startDate)
        {
            try
            {
                var transactions = _context.Transactions
                    .Where(t => (t.SorAccId == viewAccId || t.TarAccId == viewAccId) && t.Timestamp >= startDate)
                    .OrderByDescending(t => t.Timestamp)
                    .ToList();

                if (transactions == null || transactions.Count == 0)
                {
                    return NotFound("No transaction history found.");
                }

                return Ok(transactions);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}

