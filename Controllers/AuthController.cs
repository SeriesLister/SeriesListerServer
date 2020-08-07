using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeListings.Data;
using AnimeListings.Models.Requests;
using AnimeListings.Models.Responses.impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AnimeListings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<SeriesUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(
            UserManager<SeriesUser> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegistrationRequest request) 
        { 
            if (ModelState.IsValid)
            {
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

                var newUser = new SeriesUser()
                {
                    UserName = request.DisplayName,
                    Email = request.Email
                };

                var addUserTask = await _userManager.CreateAsync(newUser, request.Password);

                if (addUserTask.Succeeded)
                {
                    await AttemptRoleAdditionAsync(newUser, "User");
                    return Ok(new RegisterResponse { Success = true });
                }

                StringBuilder errors = new StringBuilder();
                foreach (var error in addUserTask.Errors)
                {
                    errors.Append(error.Description);
                    errors.Append(" : ");
                }
                return Ok(new RegisterResponse { Success = false, Error = errors.ToString(0, errors.Length - 3) });
            }
            return NotFound();
        }

        
        private async Task AttemptRoleAdditionAsync(SeriesUser user, string role)
        {
            try
            {
                var roleAdditionResult = await _userManager.AddToRoleAsync(user, role);
            } catch(Exception)
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
                await _userManager.AddToRoleAsync(user, role);
            }
        }

    }
}
