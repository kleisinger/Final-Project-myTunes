using System;
using System.Collections.Generic;

namespace Final_Project_myTunes.Models
{
    public partial class Purchase
    {
        public int Id { get; set; }
        public DateTime? DateOfPurchase { get; set; }
        public double? UserRating { get; set; }
        public int? SongId { get; set; }
        public int? UserId { get; set; }

        public virtual Song? Song { get; set; }
        public virtual User? User { get; set; }

        public Purchase(Song song, User user, double userrating)
        {
            Song = song;
            User = user;
            UserRating = userrating;
        }

        public Purchase(Song song, User user)
        {
            Song = song;
            User = user;
            DateOfPurchase = DateTime.Now;
        }

        public Purchase()
        {

        }

    }
}
