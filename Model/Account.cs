using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace cSharp_BankSystem_REST_API.Model
{
    public class Account
    {
        [Key]
        [JsonIgnore]
        public int Account_Id { get; set; }
        public string HolderName { get; set; }
        public decimal Balance { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public List<Transaction> Transactions { get; set; }
        [ForeignKey("User")]
        public int User_Id { get; set; }
    }
}
