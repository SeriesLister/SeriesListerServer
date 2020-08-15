using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeListings.Models.Responses.impl
{
    public class LoginResponse : BasicResponse
    {

        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }

    }
}
