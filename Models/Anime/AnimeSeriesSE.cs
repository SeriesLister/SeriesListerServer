using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeListings.Models.Anime
{
    public class AnimeSeriesSE
    {

        public int Id { get; set; }

        public int Season { get; set; }

        public int Episodes { get; set; }

        public AnimeSeries AnimeSeries { get; set; }

    }
}
