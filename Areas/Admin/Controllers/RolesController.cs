using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimeListings.Data;
using AnimeListings.Models.ViewModels;
using AnimeListings.ViewModels.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimeListings.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("/admin/roles")]
    public class RolesController : Controller
    {

        private readonly RoleManager<IdentityRole> rolesManager;

        private readonly DatabaseContext context;

        private readonly UserManager<SeriesUser> userManager;

        public RolesController(RoleManager<IdentityRole> RoleManager, DatabaseContext databaseContext, UserManager<SeriesUser> UserManager)
        {
            rolesManager = RoleManager;
            context = databaseContext;
            userManager = UserManager;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            return View(rolesManager.Roles);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(RoleCreateViewModel createViewModel)
        {
            if (ModelState.IsValid)
            {
                string RoleName = createViewModel.RoleName;
                var result = rolesManager.RoleExistsAsync(RoleName);

                if (!result.Result)
                {
                    await rolesManager.CreateAsync(new IdentityRole(RoleName));
                    ModelState.AddModelError("success", "Role: " + RoleName + " has been created!");
                    return View();
                }
                
                ModelState.AddModelError("error", "Role: " + RoleName + " already exists!");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await rolesManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            RoleDetailsViewModel detailViewModel = new RoleDetailsViewModel()
            {
                RoleName = role.Name,
                RoleId = role.Id
            };

            return View(detailViewModel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await rolesManager.FindByIdAsync(id);
            if (role == null || id == null)
            {
                return RedirectToAction("Index");
            }
            await rolesManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await rolesManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            RoleDetailsViewModel detailViewModel = new RoleDetailsViewModel()
            {
                RoleName = role.Name,
                RoleId = role.Id
            };

            return View(detailViewModel);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditConfirm(RoleDetailsViewModel detailsViewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var role = await rolesManager.FindByIdAsync(detailsViewModel.RoleId);

            if (role == null)
            {
                return RedirectToAction("Index");
            }

            role.Name = detailsViewModel.NewRoleName;
            await rolesManager.UpdateAsync(role);

            return RedirectToAction("Index");
        }

    }
}