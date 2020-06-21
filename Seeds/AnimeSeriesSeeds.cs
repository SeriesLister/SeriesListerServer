using AnimeListings.Data;
using AnimeListings.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AnimeListings.Seeds
{
    public static class AnimeSeriesSeeds
    {

        public static async Task SeedData(DatabaseContext context)
        {

            if (context.AnimeSeries.Any())
            {
                return;
            }

            String JsonFile = File.ReadAllText("/home/tristan/AnimeListings/AnimeListingsServer/Data/json/series-info.json");
            var root = JsonDocument.Parse(JsonFile).RootElement.GetProperty("animeData");
            for (int i = 0; i < root.GetArrayLength(); i++)
            {
                context.Add(new AnimeSeries
                {
                    EnglishTitle = root[i].GetProperty("title").ToString(),
                    Type = root[i].GetProperty("type").ToString(),
                    //Episodes = root[i].GetProperty("episodes").ToString().Contains("?") ? 0 : Int32.Parse(root[i].GetProperty("episodes").ToString()),
                    ReleaseDate = GetDateTime(root[i].GetProperty("startDate").ToString()),
                    FinishDate = GetDateTime(root[i].GetProperty("finishDate").ToString()),
                });
            }

            await context.SaveChangesAsync();

        }

        private static DateTime GetDateTime(string dateProp)
        {
            int dateLength = dateProp.Length;
            if (dateLength == 4)
            {
                return new DateTime(Int32.Parse(dateProp), 1, 1);
            }
            else if (dateProp.Contains("?") && dateLength == 0)
            {
                return new DateTime(2020, 1, 1);
            }
            else if (dateLength == 0)
            {
                return new DateTime(1910, 1, 1);
            }
            else
            {
                return DateTime.Parse(dateProp);
            }
        }

    }
}
