using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace cSharp_BankSystem_REST_API.Model
{
    public class User
    {
        [Key]
        [JsonIgnore]
        public int User_Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public List<Account> Accounts { get; set; }
    }
}
