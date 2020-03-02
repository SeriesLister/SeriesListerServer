using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody]AnimeSeries animeSeries)
        {
            if (ModelState.IsValid)
            {
                var response = new StatusReponse();
                _context.Add(animeSeries);
                await _context.SaveChangesAsync();
                response.Result = true;
                return Ok(new { response, animeSeries.Id });
            }
            return NoContent();
        }

        // GET: AnimeSeries/Edit/5
        [HttpGet("edit/{id}")]
        public async Task<ActionResult<AnimeSeries>> GetDetails(int? id)
        {
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

        // DELETE: AnimeSeries/Delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = new StatusReponse();
            if (id == 0)
            {
                return Ok(response);
            }

            var animeSeries = await _context.AnimeSeries.FindAsync(id);
            if (animeSeries == null)
            {
                return Ok(response);
            }
            _context.AnimeSeries.Remove(animeSeries);
            await _context.SaveChangesAsync();
            response.Result = true;
            return Ok(response);
        }

        private bool AnimeSeriesExists(int id)
        {
            return _context.AnimeSeries.Any(e => e.Id == id);
        }
    }
}
