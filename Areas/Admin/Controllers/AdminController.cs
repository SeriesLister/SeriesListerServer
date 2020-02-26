using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimeListings.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("/Admin/[Controller]")]
    public abstract class AdminController : Controller { }
}