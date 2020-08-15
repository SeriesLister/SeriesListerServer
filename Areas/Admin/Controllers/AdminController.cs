using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AnimeListings.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimeListings.Areas.Admin.Controllers
{
    //[PermissionsFilter(Permissions = "admin")]
    [Authorize]
    [ApiController]
    [Route("/Admin/[Controller]")]
    public abstract class AdminController : Controller { }
}