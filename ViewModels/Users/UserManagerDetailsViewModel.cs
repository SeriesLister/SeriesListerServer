using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeListings.ViewModels.Users
{
    public class UserManagerDetailsViewModel
    {

        public string Id { get; set; }

        public string Email { get; set; }

        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }
        
        public IList<String> Permissions { get; set; }
    }
}
