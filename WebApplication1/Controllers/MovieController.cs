using AList.Data;
using AList.Models;
using AList.Utility;
using AList.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace AList.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MovieController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;

            // use eager loading to load movies
            _db.Movies.Include(Movie => Movie.Actors).ToList();
        }

        // GET: MovieController
        public ActionResult Index()
        {
            IEnumerable<Movie> movies = _db.Movies.ToList();
            return View(movies);
        }

        // GET: MovieController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MovieController/Create
        public ActionResult Create()
        {
            // add contents to viewbag for use in view
            ViewBag.AvailableActors = _db.Actors.ToList().OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            ViewBag.CurrentActors = new List<Actor>(); //the partial requires an empty list

            return View();
        }

        // POST: MovieController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MovieVM vm, IFormFile? file)
        {
            try
            {
                Movie movie = vm.Movie;
                if (vm.SelectedActorIds != null)
                {
                    movie.Actors = _db.Actors.Where(m => vm.SelectedActorIds.Contains(m.Id)).ToList();
                }

                HandleImage(file, ref movie);

                _db.Add(movie);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MovieController/Edit/5
        public ActionResult Edit(int id)
        {
            // create MovieVM ViewModel object
            MovieVM vm = new MovieVM
            {
                Movie = _db.Movies.SingleOrDefault(m => m.Id == id)
            };

            // Add contents to ViewBag for use in view
            ViewBag.AvailableActors = _db.Actors.ToList().OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            ViewBag.CurrentActors = vm.Movie.Actors.ToList();

            return View(vm);
        }

        // POST: MovieController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MovieVM vm, IFormFile? file)
        {
            try
            {
                // Retrieve the existing actor from the DbContext to avoid already being tracked error
                Movie movie = _db.Movies.Find(vm.Movie.Id);
                movie.Title = vm.Movie.Title;
                movie.Director = vm.Movie.Director;

                // update actor movies
                if (vm.SelectedActorIds != null)
                {
                    movie.Actors = _db.Actors.Where(m => vm.SelectedActorIds.Contains(m.Id)).ToList();
                }
                else
                {
                    movie.Actors = null;
                }

                HandleImage(file, ref movie);

                if (ModelState.IsValid)
                {
                    _db.Update(movie);
                    _db.SaveChanges();
                    return RedirectToAction(nameof(Index));

                }
                return View(vm);

            }
            catch
            {
                return View();
            }
        }

        // GET: MovieController/Delete/5
        public ActionResult Delete(int id)
        {
            Movie movie = _db.Movies.Find(id);
            return View(movie);
        }

        // POST: MovieController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                Movie movie = _db.Movies.SingleOrDefault(m => m.Id == id);
                _db.Remove(movie);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public void HandleImage(IFormFile file, ref Movie movie)
        {
            // get root path
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\movies");
                var extension = Path.GetExtension(file.FileName);

                // if the image already exists delete the old one
                if (movie.ImageUrl != null)
                {
                    var oldImagePath = Path.Combine(wwwRootPath, movie.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
                movie.ImageUrl = @"\images\movies\" + fileName + extension;

            }
        }
    }
}
