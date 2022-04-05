using System;
using System.Collections.Generic;

namespace Final_Project_myTunes.Models
{
    public partial class User
    {
        public User()
        {
            Purchases = new HashSet<Purchase>();
        }

        public int Id { get; set; }
        public string? UserName { get; set; }
        public decimal? WalletBalance { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; }


    }
}
