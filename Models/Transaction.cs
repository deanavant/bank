using System;
using System.ComponentModel.DataAnnotations;

namespace bank.Models
{
    public class Transaction : BaseEntity
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public Transaction() { }

        public Transaction(int amt,DateTime d, int act)
        {
            Amount = amt;
            Date = d;
            AccountId = act;
        }
    }

    public class TransactionViewModel : BaseEntity
    {
        [Required(ErrorMessage = "Deposit or Withdrawl(-value) required")]
        [Display(Name = "Deposit/Withdraw:")]
        public int Amount { get; set; }
    }
}