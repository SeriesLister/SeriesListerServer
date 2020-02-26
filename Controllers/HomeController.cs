using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AnimeListings.Models;
using Microsoft.AspNetCore.Authorization;
using AnimeListings.Data;
using Microsoft.AspNetCore.Identity;
using AnimeListings.Models.ViewModels;
using System.Security.Claims;

namespace AnimeListings.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<SeriesUser> _signInManager;
        private readonly UserManager<SeriesUser> _userManager;

        public HomeController(ILogger<HomeController> logger,
            UserManager<SeriesUser> userManager,
            SignInManager<SeriesUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (IsUserLoggedIn())
            {
                return RedirectToAction("Index", "User");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (IsUserLoggedIn())
            {
                return RedirectToAction("Index", "User");
            }

            if (ModelState.IsValid)
            {
                var user = new SeriesUser()
                {
                    UserName = model.DisplayName,
                    Email = model.Email,
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    return RedirectToAction("Login");
                }

                foreach (var errors in result.Errors)
                {
                    if (errors.Code.Contains("UserName", StringComparison.OrdinalIgnoreCase))
                        ModelState.AddModelError("DisplayName", errors.Description);
                    else if (errors.Code.Contains("Email", StringComparison.OrdinalIgnoreCase))
                        ModelState.AddModelError("Email", errors.Description);
                    else
                        ModelState.AddModelError("", errors.Description);
                }
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            string redirectUrl = Url.Action("GoogleResponse", "Home");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction("Login");

            var isUserSignedIn = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (isUserSignedIn.Succeeded)
            {
                return RedirectToAction("Index", "User");
            } else
            {
                SeriesUser user = new SeriesUser
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    FirstName = info.Principal.FindFirst(ClaimTypes.GivenName).Value,
                    LastName = info.Principal.FindFirst(ClaimTypes.Surname).Value
                };
                var isUserCreated = await _userManager.CreateAsync(user);
                if (isUserCreated.Succeeded)
                {
                    var addExternalToUser = await _userManager.AddLoginAsync(user, info);
                    if (addExternalToUser.Succeeded)
                    {

                        await _userManager.AddToRoleAsync(user, "User");
                        await _signInManager.SignInAsync(user, false);
                        return RedirectToAction("Index", "User");
                    }
                }
            }

            return RedirectToAction("Login");
        }

        [HttpGet, ActionName("Login")]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            if (IsUserLoggedIn())
            {
                return RedirectToAction("Index", "User");
            }

            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            Console.WriteLine(model.ExternalLogins.Count);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (IsUserLoggedIn())
            {
                return RedirectToAction("Index", "User");
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    //is persisent is the remember me option
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "User");
                    }
                }

                ModelState.AddModelError("", "Invalid Email Address or Password");

            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            if (IsUserLoggedIn())
            {
                await _signInManager.SignOutAsync();
            }
            return Redirect("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {

            return View();
        }

        private bool IsUserLoggedIn()
        {
            return User.Identity.IsAuthenticated;
        }

    }
}
