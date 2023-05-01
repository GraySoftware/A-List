using AList.Data;
using AList.Models;
using AList.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace AList.Controllers
{
    public class ActorController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ActorController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;

            // use eager loading to load movies
            _db.Actors.Include(Actor => Actor.Movies).ToList();
        }

        // GET: ActorController
        public ActionResult Index()
        {
            IEnumerable<Actor> actorList = _db.Actors.ToList();
            return View(actorList);
        }

        // GET: ActorController/Create
        public IActionResult Create()
        {
            ViewBag.AvailableMovies = _db.Movies.ToList().OrderBy(x => x.Title).ToList();
            ViewBag.CurrentMovies = new List<Movie>(); // partial requires a list of movies

            return View();
        }

        // POST: ActorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ActorVM vm, IFormFile? file)
        {
            try
            {
                Actor actor = vm.Actor;
                if (vm.SelectedMovieIds != null)
                {
                    actor.Movies = _db.Movies.Where(m => vm.SelectedMovieIds.Contains(m.Id)).ToList();
                }

                HandleImage(file, ref actor);

                _db.Add(actor);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ActorController/Edit/5
        public ActionResult Edit(int id, string returnUrl)
        {
            // create actor ViewModel object
            ActorVM vm = new ActorVM
            {
                Actor = _db.Actors.SingleOrDefault(m => m.Id == id)
            };

            // Add contents to ViewBag for use in view
            List<Movie> test = vm.Actor.Movies.ToList();

            ViewBag.AvailableMovies = _db.Movies.ToList().OrderBy(x => x.Title).ToList();
            ViewBag.CurrentMovies = vm.Actor.Movies.ToList();

            return View(vm);
        }

        // POST: ActorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ActorVM vm, IFormFile? file)
        {
            try
            {
                // Retrieve the existing actor from the DbContext to avoid already being tracked error
                Actor actor = _db.Actors.Find(vm.Actor.Id); 
                actor.FirstName = vm.Actor.FirstName;
                actor.LastName = vm.Actor.LastName;
                actor.Age = vm.Actor.Age;

                // update actor movies
                if (vm.SelectedMovieIds != null)
                {
                    actor.Movies = _db.Movies.Where(m => vm.SelectedMovieIds.Contains(m.Id)).ToList();
                }
                else 
                {
                    actor.Movies = null;
                }

                HandleImage(file, ref actor);

                _db.Update(actor);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ActorController/Delete/5
        public ActionResult Delete(int id)
        {
            Actor obj = _db.Actors.Find(id);
            return View(obj);
        }

        // POST: ActorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Actor obj)
        {
            try
            {
                Actor actor = _db.Actors.SingleOrDefault(m => m.Id == id);
                _db.Remove(actor);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public void HandleImage(IFormFile file, ref Actor actor)
        {
            // get root path
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\actors");
                var extension = Path.GetExtension(file.FileName);

                // if the image already exists delete the old one
                if (actor.ImageUrl != null)
                {
                    var oldImagePath = Path.Combine(wwwRootPath, actor.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
                actor.ImageUrl = @"\images\actors\" + fileName + extension;

            }
        }
    }
}
