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

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshTokens(RefreshTokenView refreshTokenView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Console.WriteLine("Refreshing Token");

            SeriesUser user = await _userManager.FindByEmailAsync(refreshTokenView.Email);

            if (user == null)
            {
                return Unauthorized();
            }

            var refreshToken = _context.RefreshTokens.SingleOrDefault(m => m.Token == refreshTokenView.RefreshToken);

            if (refreshToken == null || !refreshToken.IsValid() || refreshToken.Email != user.Email)
            {
                if (refreshToken != null)
                {
                    _context.RefreshTokens.Remove(refreshToken);
                    await _context.SaveChangesAsync();
                }
                return Unauthorized();
            }

            refreshToken.Token = Guid.NewGuid().ToString();
            refreshToken.Provided = DateTime.UtcNow;

            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();
            string token = _JWTGenerator.GenerateEncodedToken(user.Id);

            return Ok(new { token, refreshToken = refreshToken.Token.ToString() });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            Console.WriteLine("email: " + model.Email + " : display: " + model.DisplayName + " : password: " + model.Password);
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
                        string token = _JWTGenerator.GenerateEncodedToken(user.Id);
                        string refreshToken = await GenerateRefreshToken(user.Email);
                        
                        return Ok(new { token, user.Email, user.UserName, refreshToken });
                    }
                }
            }
            return Problem(detail: "Invalid Email Address or Password");
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
