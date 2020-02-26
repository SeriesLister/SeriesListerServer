using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AnimeListings.Data;
using AnimeListings.Models;

namespace AnimeListings
{
    public class UserAnimeListsController : Controller
    {
        private readonly DatabaseContext _context;

        public UserAnimeListsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: UserAnimeLists
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserAnimeLists.ToListAsync());
        }

        // GET: UserAnimeLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAnimeList = await _context.UserAnimeLists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userAnimeList == null)
            {
                return NotFound();
            }

            return View(userAnimeList);
        }

        // GET: UserAnimeLists/Create
        [Authorized]
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserAnimeLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorized]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CurrentEpisode,Status")] UserAnimeList userAnimeList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userAnimeList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userAnimeList);
        }

        // GET: UserAnimeLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAnimeList = await _context.UserAnimeLists.FindAsync(id);
            if (userAnimeList == null)
            {
                return NotFound();
            }
            return View(userAnimeList);
        }

        // POST: UserAnimeLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CurrentEpisode,Status")] UserAnimeList userAnimeList)
        {
            if (id != userAnimeList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userAnimeList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserAnimeListExists(userAnimeList.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userAnimeList);
        }

        // GET: UserAnimeLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userAnimeList = await _context.UserAnimeLists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userAnimeList == null)
            {
                return NotFound();
            }

            return View(userAnimeList);
        }

        // POST: UserAnimeLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userAnimeList = await _context.UserAnimeLists.FindAsync(id);
            _context.UserAnimeLists.Remove(userAnimeList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserAnimeListExists(int id)
        {
            return _context.UserAnimeLists.Any(e => e.Id == id);
        }
    }

    internal class AuthorizedAttribute : Attribute
    {
    }
}
