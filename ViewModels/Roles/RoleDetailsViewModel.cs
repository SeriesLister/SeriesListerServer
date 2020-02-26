using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeListings.ViewModels.Roles
{
    public class RoleDetailsViewModel
    {
        public string RoleName { get; set; }

        [Display(Name = "New Role Name")]
        public string NewRoleName { get; set; }

        public string RoleId { get; set; }

        public bool UpdateAllUsers { get; set; } = true;
    }
}
