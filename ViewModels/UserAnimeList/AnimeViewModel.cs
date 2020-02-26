using AnimeListings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeListings.ViewModels.UserAnimeList
{
    public class AnimeViewModel
    {

        public IList<AnimeSeries> AnimeSeries { get; set; }

        public int LastPage { get; set; }

    }
}
