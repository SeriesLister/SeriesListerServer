using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeListings.ViewModels.Users
{
    public class UserManagerEditViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        public string Permissions { get; set; }
    }
}
