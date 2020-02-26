using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeListings.ViewModels.UserAnimeList
{
    public class AnimeEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string EnglishTitle { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public int Episodes { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public DateTime FinishDate { get; set; }
    }
}
