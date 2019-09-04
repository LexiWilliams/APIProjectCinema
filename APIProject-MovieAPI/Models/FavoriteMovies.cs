using System;
using System.Collections.Generic;

namespace APIProject_MovieAPI.Models
{
    public partial class FavoriteMovies
    {
        public int MovieId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Rated { get; set; }
        public string Runtime { get; set; }
        public string Director { get; set; }
        public string Genre { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
