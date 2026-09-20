using MovieTicketingSystem.Utility;

namespace MovieTicketingSystem.Areas.Admin.Controllers
{
    //[Area(AreaConstants.ADMIN_AREA)]

    public class CinemaController : Controller
    {
        private readonly ApplicationDbContext _db = new();

        public IActionResult Index(string? query, int page = 1, int size = 4)
        {
            var cinemas = _db.Cinemas.AsQueryable();

            if (query is not null)
                cinemas = cinemas.Where(e => e.Name.ToLower().Contains(query.ToLower()));

            var totalPages = Math.Ceiling(cinemas.Count() / (double)size);

            cinemas = cinemas.Skip((page - 1) * size).Take(size);

            return View(new CinemaWithFilterVM
            {
                Cinemas = cinemas,
                CurrentPage = page,
                Query = query ?? "",
                TotalPages = totalPages
            });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Cinema cinema, IFormFile Img)
        {
            if (Img is not null && Img.Length > 0)
            {
                var fileName = $"{Guid.NewGuid().ToString()}-{DateTime.Now.ToString("dd-MM-yy")}{Path.GetExtension(Img.FileName)}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgs", "cinemas", fileName);


                using (var Stream = System.IO.File.Create(filePath))
                {
                    Img.CopyTo(Stream);
                }
                cinema.Img = fileName;
            }

            _db.Cinemas.Add(cinema);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Update(int Id)
        {
            var cinema = _db.Cinemas.AsNoTracking().FirstOrDefault(e=>e.Id == Id);

            if (cinema is null) return RedirectToAction(nameof(HomeController.NotFoundPage), ControllerConstants.HOME_CONTROLLER);

            return View(cinema);
        }

        [HttpPost]
        public IActionResult Update(Cinema Cinema, IFormFile Img)
        {
            var cinemaDb = _db.Cinemas.AsNoTracking().FirstOrDefault(e => e.Id == Cinema.Id);

            if (cinemaDb is null) return NotFound();

            if (Img is not null && Img.Length > 0)
            {
                var fileName = $"{Guid.NewGuid().ToString()}-{DateTime.Now.ToString("dd-MM-yy")}{Path.GetExtension(Img.FileName)}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgs", "cinemas", fileName);

                using (var Stream = System.IO.File.Create(filePath))
                {
                    Img.CopyTo(Stream);
                }
                Cinema.Img = fileName;

                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgs", "cinemas", cinemaDb.Img);
                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);
                                
                Cinema.Img = fileName;
            }
            else
               Cinema.Img = cinemaDb.Img;
            
            _db.Cinemas.Update(Cinema);

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var Cinema = _db.Cinemas.FirstOrDefault(e => e.Id == id);

            if (Cinema is null) return RedirectToAction(nameof(HomeController.NotFoundPage), ControllerConstants.HOME_CONTROLLER);
            _db.Cinemas.Remove(Cinema);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));

        }
    }
}
