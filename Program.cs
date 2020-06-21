using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimeListings.Data;
using AnimeListings.Seeds;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AnimeListings
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    using (var context = services.GetService<DatabaseContext>())
                    {
                        AnimeSeriesSeeds.SeedData(context).GetAwaiter().GetResult();
                    }

                    using (var roles = services.GetService<RoleManager<IdentityRole>>())
                    {
                        RolesSeed.SeedData(roles).GetAwaiter().GetResult();
                    }
                }
                catch (Exception e)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(e, "An error occured seeding the DB");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
