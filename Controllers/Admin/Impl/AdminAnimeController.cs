using System;
using System.Linq;
using System.Threading.Tasks;
using AnimeListings.Data;
using AnimeListings.Helpers;
using AnimeListings.Models;
using AnimeListings.Models.HTTP.Requests.Anime;
using AnimeListings.Models.Responses.impl;
using AnimeListings.ViewModels.UserAnimeList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimeListings.Controllers.Admin.Impl
{
    public class AdminAnimeController : AdminController
    {
        private readonly DatabaseContext _context;
        
        public AdminAnimeController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page, string search)
        {
            var series = _context.AnimeSeries.Include(se => se.AnimeSeriesSEs).Include(pic => pic.Picture).Select(anime => anime);
            if (!string.IsNullOrEmpty(search))
            {
                series = series.Where(anime => anime.EnglishTitle.Contains(search, StringComparison.OrdinalIgnoreCase));
                //ViewData["search"] = search;
            }
            var listedAnime = await PaginatedList<AnimeSeries>.CreateAsync(series.AsNoTracking(), page ?? 1, 50);
            return Ok(new AnimeListedResponse{Success = true, AnimeSeries = listedAnime, LastPage = listedAnime.TotalPages });
        }
        
    }
}