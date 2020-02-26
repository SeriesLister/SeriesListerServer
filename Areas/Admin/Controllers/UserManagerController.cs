using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeListings.Data;
using AnimeListings.Helpers;
using AnimeListings.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnimeListings.Areas.Admin.Controllers
{
    public class UserManagerController : AdminController
    {

        private readonly UserManager<SeriesUser> UserManager;
        private readonly DatabaseContext Context;

        public UserManagerController(UserManager<SeriesUser> userManager, DatabaseContext context)
        {
            UserManager = userManager;
            Context = context;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var users = await PaginatedList<SeriesUser>.CreateAsync(UserManager.Users, page ?? 1, 50);

            var model = new List<UserListedViewModel>();

            foreach (var user in users)
            {
                var userRight = await UserManager.GetRolesAsync(user);

                var User = new UserListedViewModel()
                {
                    UserId = user.Id,
                    Email = user.Email,
                    DisplayName = user.UserName,
                    Permissions = userRight
                };
                model.Add(User);
            }
            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id.Length == 0 || id == null)
            {
                return RedirectToAction("Index", "UserManager");
            }

            var user = await UserManager.FindByIdAsync(id);

            if (user == null)
            {
                return RedirectToAction("Index", "UserManager");
            }

            var UserRights = await UserManager.GetRolesAsync(user);
            var model = new UserManagerDetailsViewModel
            {
                Email = user.Email,
                DisplayName = user.UserName,
                Id = user.Id,
                Permissions = UserRights
            };

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id.Length == 0 || id == null)
            {
                return RedirectToAction("Index", "UserManager");
            }

            var user = await UserManager.FindByIdAsync(id);

            if (user == null)
            {
                return RedirectToAction("Index", "UserManager");
            }

            var UserRights = await UserManager.GetRolesAsync(user);
            StringBuilder stringBuilder = new StringBuilder();

            for (int j = 0; j < UserRights.Count; j++)
            {
                stringBuilder.Append(UserRights[j]);
                if (UserRights.Count - 1 != j)
                {
                    stringBuilder.Append(", ");
                }
            }

            var model = new UserManagerEditViewModel
            {
                Email = user.Email,
                DisplayName = user.UserName,
                Id = user.Id,
                Permissions = stringBuilder.ToString()
            };

            return View(model);
        }


        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> ConfirmDetails(UserManagerEditViewModel model)
        {
            //All users should have User permission by default
            if (model.Permissions == null || model.Permissions.Length == 0)
            {
                return RedirectToAction("Edit", "UserManager", model.Id);
            }

            var user = await UserManager.FindByIdAsync(model.Id);
            var NewPermissions = model.Permissions.Trim().Split(", ");
            if (user != null)
            {
                user.Email = model.Email;
                user.UserName = model.DisplayName;

                var update = await UserManager.UpdateAsync(user);
                var OldPermissions = await UserManager.GetRolesAsync(user);

                var RemoveOldPermissions = await UserManager.RemoveFromRolesAsync(user, OldPermissions);
                var AddNewPermissions = await UserManager.AddToRolesAsync(user, NewPermissions);

                if (update.Succeeded && AddNewPermissions.Succeeded && RemoveOldPermissions.Succeeded)
                {
                    return RedirectToAction("Edit", "UserManager", model.Id);
                }
            }

            return View();
        }

    }
}