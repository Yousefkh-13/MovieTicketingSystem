using Microsoft.AspNetCore.Mvc;

namespace MovieTicketingSystem.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {        
        private readonly ApplicationDbContext _db = new();

        public IActionResult Index()
        {
            var model = new DashboardVM
            {
                TotalCinemas = _db.Cinemas.Count(),
                TotalMovies = _db.Movies.Count(),
                TotalActors = _db.Actors.Count(),
                TotalCategories = _db.Categories.Count()
            };

            return View(model);
        }
    }
}
