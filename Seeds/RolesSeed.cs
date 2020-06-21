using System;
using System.Threading.Tasks;
using AnimeListings.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AnimeListings.Seeds
{

    public static class RolesSeed {
    public static async Task SeedData(RoleManager<IdentityRole> roles)
        {

            var user = await roles.FindByNameAsync("User");
            var admin = await roles.FindByNameAsync("Admin");
            var siteManager = await roles.FindByNameAsync("Site Manager");
            Console.WriteLine("checking for roles!");
            if (user == null) {
                Console.WriteLine("User role is null");
                await roles.CreateAsync(new IdentityRole("Aser"));
            }
            if (admin == null) {
                Console.WriteLine("Admin role is null");
                await roles.CreateAsync(new IdentityRole("Admin"));
            }
            if (siteManager == null) {
                Console.WriteLine("Site Manager role is null");
                await roles.CreateAsync(new IdentityRole("Site Manager"));
            }
        }
    }
}