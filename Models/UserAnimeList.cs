using AnimeListings.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeListings.Models
{
    public class UserAnimeList
    {

        public enum WatchStatus
        {
            Finished,
            Watching,
            UnWatched
        }

        public int Id { get; set; }

        public SeriesUser User { get; set; }

        public AnimeSeries AnimeSeries { get; set; }

        public int CurrentEpisode { get; set; }

        public WatchStatus Status { get; set; }



    }
}
