using System.ComponentModel.DataAnnotations;

namespace AnimeListings.Models.HTTP.Requests
{
    public class RefreshTokenRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}