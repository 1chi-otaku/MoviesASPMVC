using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MoviesContext _db;

        public MoviesController(MoviesContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Movies> movies = await Task.Run(() => _db.Movies);
            ViewBag.Movies = movies;
            return View();
        }
    }
}
