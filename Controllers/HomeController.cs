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
using AnimeListings.Tasks;
using AnimeListings.ViewModels;
using AnimeListings.Models;
using System.Linq;
using AnimeListings.Models.Responses.impl;
using AnimeListings.Models.Requests;

namespace AnimeListings.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<SeriesUser> _signInManager;
        private readonly UserManager<SeriesUser> _userManager;
        private readonly DatabaseContext _context;
        private readonly JWTGenerator _JWTGenerator;

        public HomeController(ILogger<HomeController> logger,
            UserManager<SeriesUser> userManager,
            SignInManager<SeriesUser> signInManager,
            JWTGenerator jwtToken,
            DatabaseContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _JWTGenerator = jwtToken;
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
            /*if (info == null)
                return RedirectToAction("Login");*/

            var isUserSignedIn = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (isUserSignedIn.Succeeded)
            {
                //return RedirectToAction("Index", "User");
            }
            else
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
                        //return RedirectToAction("Index", "User");
                    }
                }
            }

            return null;
            /*return RedirectToAction("Login");*/
        }

        private async Task<string> GenerateRefreshToken(string email)
        {
            RefreshToken refreshToken = new RefreshToken
            {
                Email = email,
                Token = Guid.NewGuid().ToString()
            };
            var refreshSearch = _context.RefreshTokens.FirstOrDefault(m => m.Email == email);
            if (refreshSearch != null)
            {
                refreshSearch.Token = refreshToken.Token;
                _context.RefreshTokens.Update(refreshSearch);
            } else
            {
                _context.RefreshTokens.Add(refreshToken);
            }
            await _context.SaveChangesAsync();
            return refreshToken.Token.ToString();
        }
    }
}
