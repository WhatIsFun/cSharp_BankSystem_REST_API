using cSharp_BankSystem_REST_API.Model;
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
        [HttpPut("Deposit")]
        public void deposit(List<Account> userAccounts, int sourceAccountId, decimal amount)
        {

            if (userAccounts.Count == 0)
            {
                Console.WriteLine("Add account first");
                return;
            }
            else if (!userAccounts.Any(account => account.Account_Id == sourceAccountId))
            {
                Console.WriteLine("Enter a valid account number.");
                return;
            }
            using (var context = new ApplicationDbContext())
            {
                try
                {
                    string type = "Deposit";
                    DateTime Timestamp = DateTime.Now;
                    var dAccount = context.Accounts.FirstOrDefault(a => a.Account_Id == sourceAccountId);

                    if (dAccount != null)
                    {
                        dAccount.Balance += amount;
                        context.SaveChanges();

                        RecordTransaction(Timestamp, type, amount, sourceAccountId, sourceAccountId, sourceAccountId);
                        Console.WriteLine("Transaction deposit added");
                    }
                    else
                    {
                        Console.WriteLine("Invalid deposit.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }
        }
        [HttpPut("Withdraw")]
        public void withdraw(List<Account> userAccounts, int sourceAccountId, decimal amount)
        {
            if (userAccounts.Count == 0)
            {
                Console.WriteLine("Add account first");
                return;
            }
            else if (!userAccounts.Any(account => account.Account_Id == sourceAccountId))
            {
                Console.WriteLine("Enter a valid account number.");
                return;
            }
            using (var context = new ApplicationDbContext())
            {
                try
                {
                    string type = "Withdrawal";
                    DateTime Timestamp = DateTime.Now;
                    var dAccount = context.Accounts.FirstOrDefault(a => a.Account_Id == sourceAccountId);

                    if (dAccount != null)
                    {
                        dAccount.Balance -= amount;
                        context.SaveChanges();

                        RecordTransaction(Timestamp, type, amount, sourceAccountId, sourceAccountId, sourceAccountId);
                        Console.WriteLine("Transaction Withdrawal added");
                    }
                    else
                    {
                        Console.WriteLine("Invalid withdraw.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }
        }
        [HttpPut("Transfer")]
        public void transfer(List<Account> userAccounts, int sourceAccountId, int targetAccountId, decimal amount)
        {
            DateTime Timestamp = DateTime.Now;
            using (var context = new ApplicationDbContext())
            {
                try
                {
                    var sourceAccount = context.Accounts.FirstOrDefault(a => a.Account_Id == sourceAccountId);
                    var targetAccount = context.Accounts.FirstOrDefault(a => a.Account_Id == targetAccountId);

                    if (sourceAccount == null || targetAccount == null)
                    {
                        Console.WriteLine("Source or target account not found.");
                        return;
                    }
                    Console.Write("Enter the amount to transfer: ");
                    if (sourceAccount.Balance >= amount)
                    {
                        // Perform the transfer
                        string type = "Transfer";
                        //string Timestamp1 = time;


                        sourceAccount.Balance -= amount;
                        targetAccount.Balance += amount;

                        context.SaveChanges();

                        RecordTransaction(Timestamp, type, amount, sourceAccountId, targetAccountId, sourceAccountId);
                        Console.WriteLine("Transaction added");
                    }
                    else
                    {
                        Console.WriteLine("Insufficient funds for the transfer.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }
        }

        private void RecordTransaction(DateTime Timestamp1, string type, decimal amount, int sourceAccountId, int targetAccountId, int accountNum)
        {
            using (var context = new ApplicationDbContext())
            {
                try
                {
                    var transaction = new Transaction
                    {
                        Timestamp = Timestamp1,
                        Type = type,
                        Amount = amount,
                        SorAccId = sourceAccountId,
                        TarAccId = targetAccountId,
                        User_Id = accountNum
                    };

                    context.Transactions.Add(transaction);
                    int rowsAffected = context.SaveChanges();

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
        }
        [HttpGet("Recording")]
        public void ViewTransactionHistory(User authenticatedUser, int viewAccId, DateTime startDate)
        {
            using (var context = new ApplicationDbContext())
            {
                try
                {
                    var transactions = context.Transactions
                        .Where(t => (t.SorAccId == viewAccId || t.TarAccId == viewAccId) && t.Timestamp >= startDate)
                        .OrderByDescending(t => t.Timestamp)
                        .ToList();
                    if (transactions == null)
                    {
                        Console.WriteLine("No transaction history found.");
                    }
                    else
                    {

                        foreach (var transaction in transactions)
                        {
                            Console.WriteLine($"Transaction ID: {transaction.T_Id}");
                            Console.WriteLine($"Timestamp:      {transaction.Timestamp}");
                            Console.WriteLine($"Type:           {transaction.Type}");
                            Console.WriteLine($"Amount:         {transaction.Amount} OMR");
                            Console.WriteLine($"Source Account: {transaction.SorAccId}");
                            Console.WriteLine($"Target Account: {transaction.TarAccId}");
                            Console.WriteLine("---------------------------");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                }
            }
        }
    }
}

