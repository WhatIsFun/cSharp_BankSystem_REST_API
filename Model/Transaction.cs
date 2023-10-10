using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public Account Account { get; set; }
        [ForeignKey("Account")]
        public int User_Id { get; set; }
    }
}
