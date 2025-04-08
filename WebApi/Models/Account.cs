using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        public string AccountType { get; set; } 
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
