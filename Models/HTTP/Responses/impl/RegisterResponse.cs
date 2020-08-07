using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeListings.Models.Responses.impl
{
    public class RegisterResponse : BasicResponse
    {

        public string DisplayNameError { get; set; }

        public string PasswordError { get; set; }

    }
}
