using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AList.Models;
using AList.Models.ViewModels;
using AList.Data;
using Microsoft.EntityFrameworkCore;

namespace AList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _db = db;
            _logger = logger;

            // use eager loading to load movies/actors
            _db.Movies.Include(Movie => Movie.Actors).ToList();
            _db.Actors.Include(Actor => Actor.Movies).ToList();
        }

        public IActionResult ActorIndex()
        {
            AListVM aListVM = new AListVM
            {
                // sort lists for display
                Actors = _db.Actors.ToList().OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList(),
                Movies = _db.Movies.ToList().OrderBy(x => x.Title).ToList()
            };
            return View(aListVM);
        }

        public IActionResult MovieIndex()
        {
            AListVM aListVM = new AListVM
            {
                // sort lists for display
                Actors = _db.Actors.ToList().OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList(),
                Movies = _db.Movies.ToList().OrderBy(x => x.Title).ToList()
            };
            return View(aListVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}