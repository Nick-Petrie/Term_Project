using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TermProject.Models;

namespace TermProject.Controllers
{
    public class MovieReviewsController : Controller
    {
        private readonly MovieReviewContext _context;

        public MovieReviewsController(MovieReviewContext context)
        {
            _context = context;
        }

        // GET: MovieReviews
        public async Task<IActionResult> Index(string searchString, string genreFilter, string sortOrder, int? page)
        {
            // ViewData stores current search/filter/sort state for the view
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentGenre"] = genreFilter;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["RatingSortParm"] = sortOrder == "Rating" ? "rating_desc" : "Rating";

            var movieReviews = _context.MovieReviews
                                       .Include(m => m.Genre)
                                       .AsQueryable();

            // Apply filtering by title
            if (!string.IsNullOrEmpty(searchString))
            {
                movieReviews = movieReviews.Where(r => EF.Functions.Like(r.MovieTitle, $"%{searchString}%"));
            }

            // Apply filtering by genre
            if (!string.IsNullOrEmpty(genreFilter))
            {
                movieReviews = movieReviews.Where(r => r.Genre != null && r.Genre.Name == genreFilter);
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "title_desc":
                    movieReviews = movieReviews.OrderByDescending(r => r.MovieTitle);
                    break;
                case "Rating":
                    movieReviews = movieReviews.OrderBy(r => r.Rating);
                    break;
                case "rating_desc":
                    movieReviews = movieReviews.OrderByDescending(r => r.Rating);
                    break;
                default:
                    movieReviews = movieReviews.OrderBy(r => r.MovieTitle);
                    break;
            }

            // Pagination
            int pageSize = 5;
            int pageNumber = page ?? 1;
            int totalReviews = movieReviews.Count();

            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalReviews / pageSize);
            ViewData["CurrentPage"] = pageNumber;

            var pagedReviews = await movieReviews
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return View(pagedReviews);
        }

        // GET: MovieReviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MovieReviews == null)
            {
                return NotFound();
            }

            var movieReview = await _context.MovieReviews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieReview == null)
            {
                return NotFound();
            }

            return View(movieReview);
        }

        [Authorize]
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,MovieTitle,ReviewerName,Rating,ReviewText,GenreId")] MovieReview movieReview)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movieReview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movieReview.GenreId);
            return View(movieReview);
        }

        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MovieReviews == null)
            {
                return NotFound();
            }

            var movieReview = await _context.MovieReviews.FindAsync(id);
            if (movieReview == null)
            {
                return NotFound();
            }

            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movieReview.GenreId);
            return View(movieReview);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MovieTitle,ReviewerName,Rating,ReviewText,GenreId")] MovieReview movieReview)
        {
            if (id != movieReview.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movieReview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieReviewExists(movieReview.Id))
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

            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movieReview.GenreId);
            return View(movieReview);
        }

        // Restrict Delete to Administrator and Manager roles
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MovieReviews == null)
            {
                return NotFound();
            }

            var movieReview = await _context.MovieReviews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieReview == null)
            {
                return NotFound();
            }

            return View(movieReview);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MovieReviews == null)
            {
                return Problem("Entity set 'MovieReviewContext.MovieReviews' is null.");
            }
            var movieReview = await _context.MovieReviews.FindAsync(id);
            if (movieReview != null)
            {
                _context.MovieReviews.Remove(movieReview);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Dashboard(string searchString, string genreFilter, string sortOrder)
        {
            // Store the current filter and sorting state in ViewData
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentGenre"] = genreFilter;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["RatingSortParm"] = sortOrder == "Rating" ? "rating_desc" : "Rating";

            var movieReviews = _context.MovieReviews
                                       .Include(m => m.Genre)
                                       .AsQueryable();

            // Apply filtering by search string
            if (!string.IsNullOrEmpty(searchString))
            {
                movieReviews = movieReviews.Where(r => EF.Functions.Like(r.MovieTitle, $"%{searchString}%"));
            }

            // Apply filtering by genre
            if (!string.IsNullOrEmpty(genreFilter))
            {
                movieReviews = movieReviews.Where(r => r.Genre != null && r.Genre.Name == genreFilter);
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "title_desc":
                    movieReviews = movieReviews.OrderByDescending(r => r.MovieTitle);
                    break;
                case "Rating":
                    movieReviews = movieReviews.OrderBy(r => r.Rating);
                    break;
                case "rating_desc":
                    movieReviews = movieReviews.OrderByDescending(r => r.Rating);
                    break;
                default:
                    movieReviews = movieReviews.OrderBy(r => r.MovieTitle);
                    break;
            }

            var viewModel = new DashboardViewModel
            {
                MovieReviews = movieReviews.ToList(),
                Genres = _context.Genres.ToList()
            };

            return View(viewModel);
        }

        private bool MovieReviewExists(int id)
        {
            return (_context.MovieReviews?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}