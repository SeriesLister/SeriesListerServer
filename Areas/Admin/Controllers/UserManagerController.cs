using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeListings.Data;
using AnimeListings.Helpers;
using AnimeListings.ViewModels;
using AnimeListings.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnimeListings.Areas.Admin.Controllers
{
    [ApiController]
    public class UserManagerController : AdminController
    {

        private readonly UserManager<SeriesUser> UserManager;

        public UserManagerController(UserManager<SeriesUser> userManager)
        {
            UserManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserListedViewModel>>> Index(int? page)
        {
            var users = await PaginatedList<SeriesUser>.CreateAsync(UserManager.Users, page ?? 1, 50);

            var userModel = new List<UserListedViewModel>();

            foreach (var user in users)
            {
                var userRight = await UserManager.GetRolesAsync(user);

                var User = new UserListedViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    DisplayName = user.UserName,
                    Permissions = userRight
                };
                userModel.Add(User);
            }
            return Ok(new { userModel, users.TotalPages });
        }

        [HttpGet("edit/{id}")]
        public async Task<ActionResult<UserManagerEditViewModel>> Edit(string id)
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

            return Ok(model);
        }


        [HttpPut("edit/{id}")]
        public async Task<ActionResult> ConfirmDetails(UserManagerEditViewModel model)
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

                var response = new StatusReponse();
                var update = await UserManager.UpdateAsync(user);
                var OldPermissions = await UserManager.GetRolesAsync(user);

                var RemoveOldPermissions = await UserManager.RemoveFromRolesAsync(user, OldPermissions);
                var AddNewPermissions = await UserManager.AddToRolesAsync(user, NewPermissions);

                if (update.Succeeded && AddNewPermissions.Succeeded && RemoveOldPermissions.Succeeded)
                {
                    response.Result = true;
                    return Ok(response);
                }
            }

            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id.Length == 0 || id == null)
            {
                return NoContent();
            }

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return NoContent();
            }

            var deletePending = await UserManager.DeleteAsync(user);
            if (deletePending.Succeeded)
            {
                var response = new StatusReponse
                {
                    Result = true
                };
                return Ok(response);
            }
            return NoContent();
        }

    }
}