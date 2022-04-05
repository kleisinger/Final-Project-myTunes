using System;
using System.Collections.Generic;

namespace Final_Project_myTunes.Models
{
    public partial class Song
    {
        public Song()
        {
            Purchases = new HashSet<Purchase>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Cost { get; set; }
        public double? AverageRating { get; set; }
        public int? ArtistId { get; set; }

        public virtual Artist? Artist { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }


        public void CalculateAvg()
        {
            AverageRating = Purchases.Average(p => p.UserRating);
        }

        public void BuySong(Purchase newPurchase)
        {
            Purchases.Add(newPurchase);
        }


    }
}
