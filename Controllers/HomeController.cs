using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using AnimeListings.Data;
using Microsoft.AspNetCore.Identity;
using AnimeListings.Models.ViewModels;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace AnimeListings.Controllers
{
    [ApiController]
    [Route("[Controller]")]
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

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            Console.WriteLine("email: " + model.Email + " : display: " + model.DisplayName + " : password: "+ model.Password);
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
                    return Ok();
                }

                StringBuilder errors = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    errors.Append(error.Description);
                    errors.Append(" : ");
                }

                return Problem(detail: errors.ToString(0, errors.Length - 3));
            }
            return NotFound();
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

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                    if (result.Succeeded)
                    {
                        var token = GenerateJSONWebToken(user);
                        return Ok(new { token, user.Email, user.UserName });
                    }
                }
            }

            return Problem(detail: "Invalid Email Address or Password");
        }

        private string GenerateJSONWebToken(SeriesUser user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id.ToString())
            };
            var tokeOptions = new JwtSecurityToken(
                issuer: "http://localhost:44314",
                audience: "http://localhost:44314",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        }

    }
}
