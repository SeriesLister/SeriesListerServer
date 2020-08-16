namespace AnimeListings.Models.HTTP.Requests.Anime
{
    public struct AnimeListedRequest
    {
        public int? Page { get; set; }

        public string Search { get; set; }
    }
}