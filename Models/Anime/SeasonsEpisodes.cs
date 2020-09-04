using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeListings.Models.Anime
{
    public class SeasonsEpisodes
    {

        public int Id { get; set; }

        public int Season { get; set; }

        public int Episodes { get; set; }

        [ForeignKey("AnimeSeriesId")]
        public int AnimeSeriesId { get; set; }

    }
}
