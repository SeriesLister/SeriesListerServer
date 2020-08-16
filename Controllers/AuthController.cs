using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeListings.Data;
using AnimeListings.Models;
using AnimeListings.Models.HTTP.Requests;
using AnimeListings.Models.Requests;
using AnimeListings.Models.Responses;
using AnimeListings.Models.Responses.impl;
using AnimeListings.Tasks;
using AnimeListings.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AnimeListings.Controllers
{
    public class AuthController : BaseController
    {

        private readonly DatabaseContext _context;
        private readonly UserManager<SeriesUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<SeriesUser> _signInManager;
        private readonly JWTGenerator _jwtGenerator;

        public AuthController(
            UserManager<SeriesUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<SeriesUser> signInManager,
            DatabaseContext context,
            JWTGenerator jwtToken
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            _jwtGenerator = jwtToken;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegistrationRequest request) 
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            
            if (request.DisplayName.Length < 4)
            {
                return Ok(new RegisterResponse { Success = false, DisplayNameError = "Display name has to be 4 characters or longer" });
            } else if (request.DisplayName.Length > 16)
            {
                return Ok(new RegisterResponse { Success = false, DisplayNameError = "Display name has to be 4 to 16 characters long" });
            }

            if (request.Password.Length > 32)
            {
                return Ok(new RegisterResponse { Success = false, PasswordError = "Password has to be 8 to 32 characters long" });
            }

            SeriesUser newUser = new SeriesUser()
            {
                UserName = request.DisplayName,
                Email = request.Email
            };

            IdentityResult addUserTask = await _userManager.CreateAsync(newUser, request.Password);

            if (addUserTask.Succeeded)
            {
                await AttemptRoleAdditionAsync(newUser, "User");
                return Ok(new RegisterResponse { Success = true });
            }

            StringBuilder errors = new StringBuilder();
            foreach (IdentityError error in addUserTask.Errors)
            {
                errors.Append(error.Description);
                errors.Append(" : ");
            }
            return Ok(new RegisterResponse { Success = false, Error = errors.ToString(0, errors.Length - 3) });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid) return NotFound();
            
            SeriesUser user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Ok(new LoginResponse {Success = false, Error = "Invalid email or password."});
            }

            SignInResult loginResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!loginResult.Succeeded)
            {
                return Ok(new LoginResponse {Success = false, Error = "Invalid email or password."});
            }

            string token = _jwtGenerator.GenerateEncodedToken(user.Id);
            string refreshToken = await GenerateRefreshToken(user.Email);
            
            return Ok(new LoginResponse
            {
                Email = user.Email,
                Username = user.UserName,
                RefreshToken = refreshToken,
                Token = token,
                Success = true
            });
        }

        [HttpPost("CheckUsername")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckUsername(string name)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            SeriesUser isNameTaken = await _userManager.FindByNameAsync(name);
            return Ok(new BasicResponse { Success = isNameTaken == null });
        }

        [HttpPost("CheckEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckEmail(string email)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            SeriesUser isEmailTaken = await _userManager.FindByEmailAsync(email);
            return Ok(new BasicResponse { Success = isEmailTaken == null });
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshTokens(RefreshTokenRequest response)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Console.WriteLine("Refreshing Token");

            SeriesUser user = await _userManager.FindByEmailAsync(response.Email);

            if (user == null)
            {
                return Unauthorized();
            }

            RefreshToken refreshToken = _context.RefreshTokens.SingleOrDefault(m => m.Token == response.RefreshToken);

            if (refreshToken == null || !refreshToken.IsValid() || refreshToken.Email != user.Email)
            {
                if (refreshToken == null)
                {
                    return Unauthorized();
                }
                
                _context.RefreshTokens.Remove(refreshToken);
                await _context.SaveChangesAsync();
                return Unauthorized();
            }

            refreshToken.Token = Guid.NewGuid().ToString();
            refreshToken.Provided = DateTime.UtcNow;

            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();
            string token = _jwtGenerator.GenerateEncodedToken(user.Id);
            Console.WriteLine("Sending new token!");
            return Ok(new TokensResponse{ Token = token, RefreshToken = refreshToken.Token, Success = true });
        }
        
        private async Task AttemptRoleAdditionAsync(SeriesUser user, string role)
        {
            try
            {
                await _userManager.AddToRoleAsync(user, role);
            } catch(Exception)
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
                await _userManager.AddToRoleAsync(user, role);
            }
        }

        private async Task<string> GenerateRefreshToken(string email)
        {
            RefreshToken refreshToken = new RefreshToken
            {
                Email = email,
                Token = Guid.NewGuid().ToString()
            };
            RefreshToken refreshSearch = _context.RefreshTokens.FirstOrDefault(m => m.Email == email);
            if (refreshSearch != null)
            {
                refreshSearch.Token = refreshToken.Token;
                _context.RefreshTokens.Update(refreshSearch);
            }
            else
            {
                await _context.RefreshTokens.AddAsync(refreshToken);
            }
            await _context.SaveChangesAsync();
            return refreshToken.Token;
        }
    }
}
