using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeListings.Models
{
    public class AnimeSeries
    {
        public int Id { get; set; }

        [Display(Name = "English Title")]
        public string EnglishTitle { get; set; }

        public string Type { get; set; }

        public int Episodes { get; set; }

        [Display(Name = "Air Date")]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Finish Date")]
        public DateTime FinishDate { get; set; }
    }
}
