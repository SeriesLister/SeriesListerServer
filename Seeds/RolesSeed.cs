using System;
using AnimeListings.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AnimeListings.Seeds
{

    public static class RolesSeed {
    public static async void SeedData(IServiceProvider serviceProvider)
        {
            using var context = new DatabaseContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<DatabaseContext>>());

            using var roles = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var user = await roles.FindByNameAsync("user");
            var admin = await roles.FindByNameAsync("admin");
            var siteManager = await roles.FindByNameAsync("admin");
            if (user == null) {
                await roles.CreateAsync(new IdentityRole("user"));
            }
            if (admin == null) {
                await roles.CreateAsync(new IdentityRole("admin"));
            }
            if (siteManager == null) {
                await roles.CreateAsync(new IdentityRole("sitemanager"));
            }
            await context.SaveChangesAsync();
        }
    }
}