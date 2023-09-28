using System.ComponentModel.DataAnnotations;

namespace cSharp_BankSystem_REST_API.Model
{
    public class User
    {
        [Key]
        public int User_Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Account> Accounts { get; set; }
    }
}
