﻿using System;
using System.Collections.Generic;

namespace Final_Project_myTunes.Models
{
    public partial class Artist
    {
        public Artist()
        {
            Songs = new HashSet<Song>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Song> Songs { get; set; }
    }
}
