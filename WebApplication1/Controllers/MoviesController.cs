using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MoviesContext _db;
        private readonly IWebHostEnvironment _appEnvironment;

        public MoviesController(MoviesContext db, IWebHostEnvironment appEnvironment)
        {
            _db = db;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Movies> movies = await Task.Run(() => _db.Movies);
            ViewBag.Movies = movies;
            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _db.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _db.Movies.FindAsync(id);
            if (student != null)
            {
                _db.Movies.Remove(student);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(1000000000)]
        public async Task<IActionResult> Create([Bind("Id,Title,Year,Director,Genre,Info")] Movies movie, IFormFile uploadedFile)
        {

            if (ModelState.IsValid)
            {
                if (uploadedFile != null)
                {
                    string path = "/movies/" + uploadedFile.FileName;

                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }


                    movie.Img = path;


                }
                _db.Add(movie);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        private bool MovieExists(int id)
        {
            return _db.Movies.Any(e => e.Id == id);
        }

        public IActionResult Details(int id)
        {
            var movie = _db.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movies = await _db.Movies.FindAsync(id);
            if (movies == null)
            {
                return NotFound();
            }
            return View(movies);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Year,Director,Genre,Info,Img")] Movies movie, IFormFile uploadedFile)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            string oldImagePath = movie.Img;

            if (uploadedFile != null)
            {
                string path = "/movies/" + uploadedFile.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                movie.Img = path;
            }

            if (ModelState.IsValid || uploadedFile == null)
            {
                try
                {
                    if (uploadedFile == null)
                    {
                        movie.Img = oldImagePath;
                    }

                    _db.Update(movie);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            return View(movie);
        }

    }
}
