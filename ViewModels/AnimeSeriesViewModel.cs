using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeListings.ViewModels
{
    public class AnimeSeriesViewModel
    {
        public int Id { get; set; }

        public string EnglishTitle { get; set; }

        public string Type { get; set; }

        public int Episodes { get; set; }

        public DateTime ReleaseDate { get; set; }

        public DateTime FinishDate { get; set; }

        public string ImageData { get; set; }
    }
}
