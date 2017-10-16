using System.Collections.Generic;
using System;

namespace bank.Models
{
    public class Account : BaseEntity
    {
        public int Id { get; set; }
        public int Balance { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Transaction> Transactions { get; set; }

        public Account()
        {
            Transactions = new List<Transaction>();
        }

        public Account(int b, int u)
        {
            Transactions = new List<Transaction>();
            Balance = b;
            UserId = u;
        }
    }

    
}