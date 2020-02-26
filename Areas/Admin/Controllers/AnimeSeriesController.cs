using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AnimeListings.Data;
using AnimeListings.Models;
using AnimeListings.Helpers;
using AnimeListings.Areas.Admin.Controllers;
using AnimeListings.ViewModels.UserAnimeList;
using AnimeListings.ViewModels;

namespace AnimeListings.Controllers
{
    [ApiController]
    public class AnimeSeriesController : AdminController
    {
        private readonly DatabaseContext _context;

        public AnimeSeriesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: AnimeSeries
        [HttpGet]
        public async Task<ActionResult<AnimeViewModel>> Index(int? page, string search)
        {
            var series = from anime in _context.AnimeSeries
                         select anime;

            if (!String.IsNullOrEmpty(search))
            {
                series = series.Where(series => series.EnglishTitle.Contains(search, StringComparison.OrdinalIgnoreCase));
                ViewData["search"] = search;
            }
            var seriesTest = await PaginatedList<AnimeSeries>.CreateAsync(series.AsNoTracking(), page ?? 1, 50);
            var result = new AnimeViewModel
            {
                AnimeSeries = seriesTest,
                LastPage = seriesTest.TotalPages
            };
            return result;
        }

        // GET: AnimeSeries/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var animeSeries = await _context.AnimeSeries
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (animeSeries == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(animeSeries);
        //}

        // GET: AnimeSeries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AnimeSeries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EnglishTitle,Type,Episodes,ReleaseDate,FinishDate")] AnimeSeries animeSeries)
        {
            if (ModelState.IsValid)
            {
                _context.Add(animeSeries);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(animeSeries);
        }

        // GET: AnimeSeries/Edit/5
        [HttpGet("edit/{id}")]
        public async Task<ActionResult<AnimeSeries>> GetDetails(int? id)
        {
            Console.WriteLine("ID: " + id);
            if (id == null)
            {
                return NoContent();
            }

            var animeSeries = await _context.AnimeSeries.FindAsync(id);
            if (animeSeries == null)
            {
                return NoContent();
            }

            return animeSeries;
        }

        // POST: AnimeSeries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Edit(int? id, AnimeSeries animeSeries)
        {
            if (id == null || id != animeSeries.Id)
            {
                Console.WriteLine("Id dont match on put edit request");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var reponse = new StatusReponse();
                if (!AnimeSeriesExists(animeSeries.Id))
                {
                    return NotFound();
                }
                 
                try
                {
                    _context.Update(animeSeries);
                    await _context.SaveChangesAsync();
                    reponse.Result = true;
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return Ok(reponse);
            }
            return NotFound();
        }

        // GET: AnimeSeries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animeSeries = await _context.AnimeSeries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animeSeries == null)
            {
                return NotFound();
            }

            return View(animeSeries);
        }

        // POST: AnimeSeries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animeSeries = await _context.AnimeSeries.FindAsync(id);
            _context.AnimeSeries.Remove(animeSeries);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimeSeriesExists(int id)
        {
            return _context.AnimeSeries.Any(e => e.Id == id);
        }
    }
}
