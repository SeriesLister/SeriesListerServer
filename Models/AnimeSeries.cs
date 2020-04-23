using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Column(TypeName = "Date")]//converts to short time 'MM/DD/YYYY'
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Finish Date")]
        [Column(TypeName = "Date")]//converts to short time 'MM/DD/YYYY'
        public DateTime FinishDate { get; set; }

        public byte[] ImageData { get; set; }
    }
}
