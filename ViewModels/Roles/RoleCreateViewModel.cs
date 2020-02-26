using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeListings.Models.ViewModels
{
    public class RoleCreateViewModel
    {

        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        public string Error { get; set; }
        public string Success { get; set; }

    }
}
