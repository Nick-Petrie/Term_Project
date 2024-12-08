using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TermProject.Models;

namespace TermProject.Controllers
{
    public class SubscribersController : Controller
    {
        private readonly MovieReviewContext _context;

        public SubscribersController(MovieReviewContext context)
        {
            _context = context;
        }

        // GET: Subscribers
        public async Task<IActionResult> Index()
        {
              return _context.Subscribers != null ? 
                          View(await _context.Subscribers.ToListAsync()) :
                          Problem("Entity set 'MovieReviewContext.Subscribers'  is null.");
        }

        // GET: Subscribers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Subscribers == null)
            {
                return NotFound();
            }

            var subscribers = await _context.Subscribers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (subscribers == null)
            {
                return NotFound();
            }

            return View(subscribers);
        }

        // GET: Subscribers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Subscribers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,GenderIdentity,Address,City,State,Zip,email,phoneNumber")] Subscribers subscribers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subscribers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subscribers);
        }

        // GET: Subscribers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Subscribers == null)
            {
                return NotFound();
            }

            var subscribers = await _context.Subscribers.FindAsync(id);
            if (subscribers == null)
            {
                return NotFound();
            }
            return View(subscribers);
        }

        // POST: Subscribers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,GenderIdentity,Address,City,State,Zip,email,phoneNumber")] Subscribers subscribers)
        {
            if (id != subscribers.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subscribers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscribersExists(subscribers.ID))
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
            return View(subscribers);
        }

        // GET: Subscribers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Subscribers == null)
            {
                return NotFound();
            }

            var subscribers = await _context.Subscribers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (subscribers == null)
            {
                return NotFound();
            }

            return View(subscribers);
        }

        // POST: Subscribers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Subscribers == null)
            {
                return Problem("Entity set 'MovieReviewContext.Subscribers'  is null.");
            }
            var subscribers = await _context.Subscribers.FindAsync(id);
            if (subscribers != null)
            {
                _context.Subscribers.Remove(subscribers);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubscribersExists(int id)
        {
          return (_context.Subscribers?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
