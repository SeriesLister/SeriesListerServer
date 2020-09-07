using AnimeListings.Filters;
using Microsoft.AspNetCore.Authorization;

namespace AnimeListings.Controllers.Admin
{
    [Authorize]
    [PermissionsFilter(Permissions = "Admin")]
    public class AdminController : BaseController
    {
        
    }
}