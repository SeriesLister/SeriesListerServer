using AnimeListings.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AnimeListings.Filters
{
    public class PermissionsFilter : ActionFilterAttribute, IAsyncActionFilter
    {
        public string Permissions { get; set; }

        public PermissionsFilter()
        {
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            Console.WriteLine("User ID: " + userId);
            if (String.IsNullOrEmpty(Permissions) || userId == null)
            {
                SetErrorCode(context, 401);
                return;
            }

            string[] roles = Permissions.Split(",");
            UserManager<SeriesUser> _userManager = 
                (UserManager<SeriesUser>)context.HttpContext.RequestServices.GetService(typeof(UserManager<SeriesUser>));

            SeriesUser user = await _userManager.FindByIdAsync(userId.Value);
            if (user == null)
            {
                SetErrorCode(context, 401);
                return;
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            
            foreach (string role in roles)
            {
                if (userRoles.Where(user => role.Contains(user, StringComparison.OrdinalIgnoreCase)).Any())
                {
                    await next();
                    return;
                }
            }

            SetErrorCode(context, 403);
            return;
        }

        private IActionResult SetErrorCode(ActionExecutingContext context, int code)
        {
            return context.Result = new ContentResult()
            {
                StatusCode = code
            };
        }
    }
}
