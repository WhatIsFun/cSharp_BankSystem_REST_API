using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace cSharp_BankSystem_REST_API.Model
{
    public class Account
    {
        [Key]
        public int Account_Id { get; set; }
        public string HolderName { get; set; }
        public decimal Balance { get; set; }
        public User User { get; set; }
        public List<Transaction> Transactions { get; set; }
        [ForeignKey("User")]
        public int User_Id { get; set; }
    }
}
