using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }  // Deposit, Withdraw, Transfer
        public decimal Amount { get; set; }
        public decimal AccountBalance { get; set; }
        public DateTime Date { get; set; }
        public int AccountId { get; set; }
    }
}
