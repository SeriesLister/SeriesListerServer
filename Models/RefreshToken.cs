using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeListings.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public bool Revoked { get; set; }
        public DateTime Provided { get; set; } = DateTime.UtcNow;

        public bool IsValid()
        {
            if (Revoked)
            {
                return false;
            }
            DateTime ageLimit = DateTime.UtcNow;
            ageLimit = ageLimit.AddDays(-7);
            return Provided > ageLimit;
        }
    }
}
