namespace AnimeListings.Models.Responses.impl
{
    public class TokensResponse : BasicResponse
    {
        public string Token { get; set; }
        
        public string RefreshToken { get; set; }
    }
}