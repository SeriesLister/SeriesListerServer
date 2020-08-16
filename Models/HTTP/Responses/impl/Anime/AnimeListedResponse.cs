using System.Collections.Generic;

namespace AnimeListings.Models.Responses.impl
{
    public class AnimeListedResponse : BasicResponse
    {
        public IList<AnimeSeries> AnimeSeries { get; set; }

        public int LastPage { get; set; }
    }
}