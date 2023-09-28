using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace cSharp_BankSystem_REST_API.Model
{
    public class Transaction
    {
        [Key]
        public int T_Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public int SorAccId { get; set; }
        public int TarAccId { get; set; }
        public Account Account { get; set; }
        [ForeignKey("Account")]
        public int User_Id { get; set; }
    }
}
