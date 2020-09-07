using System;
using System.Linq;
using System.Threading.Tasks;
using AnimeListings.Data;
using AnimeListings.Helpers;
using AnimeListings.Models;
using AnimeListings.Models.Responses;
using AnimeListings.Models.Responses.impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
            var series = _context.AnimeSeries.Include(se => se.SeasonsEpisodes).Select(anime => anime);
            if (!string.IsNullOrEmpty(search))
            {
                series = series.Where(anime => anime.EnglishTitle.Contains(search, StringComparison.OrdinalIgnoreCase));
                //ViewData["search"] = search;
            }
            var listedAnime = await PaginatedList<AnimeSeries>.CreateAsync(series.AsNoTracking(), page ?? 1, 50);
            return Ok(new AnimeListedResponse{Success = true, AnimeSeries = listedAnime, TotalPages = listedAnime.TotalPages });
        }
        
        [HttpGet("details")]
        public async Task<ActionResult> Details(int id)
        {
            AnimeSeries animeSeries = await _context.AnimeSeries.Include(se => se.SeasonsEpisodes).Include(pic => pic.Picture).FirstOrDefaultAsync(s => s.Id == id);
            if (animeSeries == null)
            {
                return Ok(new AnimeResponse{Success = false, Error = "Invalid Anime ID"});
            }
            return Ok(new AnimeResponse{Success = true, AnimeSeries = animeSeries});
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create(AnimeSeries series)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            AnimeSeries findExisting = await _context.AnimeSeries.
                Include(p => p.Picture).
                Include(se => se.SeasonsEpisodes).
                FirstOrDefaultAsync(s =>
                s.EnglishTitle.Equals(series.EnglishTitle, StringComparison.OrdinalIgnoreCase) ||
                s.JapaneseName.Equals(series.JapaneseName, StringComparison.OrdinalIgnoreCase
            ));

            if (findExisting == null)
            {
                var asyncAdded = _context.AddAsync(new AnimeSeries().ToModel(series));
                if (asyncAdded.IsCompletedSuccessfully)
                {
                    await _context.SaveChangesAsync();
                    return Ok(new BasicResponse{ Success = true});
                }
            }
            return Ok(new BasicResponse{Success = false, Error = "Series already exists"} );
        }

        [HttpPatch("update")]
        public async Task<ActionResult> Update(AnimeSeries series)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            
            if (series == null)
            {
                return Ok(new BasicResponse {Success = false, Error = "series information is missing" });
            }
            
            AnimeSeries animeSeries = await _context.AnimeSeries.
                Include(e => e.SeasonsEpisodes).
                Include(p => p.Picture).
                SingleOrDefaultAsync(s => s.Id == series.Id);
            if (animeSeries == null)
            {
                return Ok(new BasicResponse {Success = false, Error = "The anime series was deleted prior to request" });
            }

            if (!String.IsNullOrWhiteSpace(series.EnglishTitle))
                animeSeries.EnglishTitle = series.EnglishTitle;

            animeSeries.JapaneseName = series.JapaneseName;
            animeSeries.Type = series.Type;
            animeSeries.SeasonsEpisodes[0].Episodes = series.SeasonsEpisodes[0].Episodes;
            animeSeries.Synopsis = series.Synopsis;
            
            if (String.IsNullOrWhiteSpace(series.ReleaseDate.ToShortDateString()))
                animeSeries.ReleaseDate = series.ReleaseDate;
                
            if (String.IsNullOrWhiteSpace(series.FinishDate.ToShortDateString()))
                animeSeries.FinishDate = series.FinishDate;
            
            if (series.Picture.ImageData.Length > 0)
                animeSeries.Picture.ImageData = series.Picture.ImageData;
            
            await _context.SaveChangesAsync();
            return Ok(new BasicResponse {Success = true});
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id < 1)
            {
                return Ok(new BasicResponse {Success = false, Error = "Anime ID less than 1"});
            }

            AnimeSeries animeSeries = await _context.AnimeSeries.Include(p => p.Picture).Include(se => se.SeasonsEpisodes).FirstOrDefaultAsync(s => s.Id == id);
            if (animeSeries == null)
            {
                return Ok(new BasicResponse {Success = false, Error = "Couldn't find anime series with specified ID"});
            }

            _context.AnimeSeries.Remove(animeSeries);
            _context.AnimeSeriesPicture.Remove(animeSeries.Picture);
            await _context.SaveChangesAsync();
            return Ok(new BasicResponse {Success = true});
        }

    }
}