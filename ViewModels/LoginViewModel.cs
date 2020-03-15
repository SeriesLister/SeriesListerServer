using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeListings.Models.ViewModels
{
    public class LoginViewModel
    {

        [Required]
        [EmailAddress]
        public string Email { set; get; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; } = true;

    }
}
