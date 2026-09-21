using MovieTicketingSystem.Repositories.IRepositories;

namespace MovieTicketingSystem.Areas.Admin.Controllers
{
    [Area(AreaConstants.ADMIN_AREA)]

    public class CinemaController : Controller
    {
        //private readonly ApplicationDbContext _db = new();

        private readonly IRepository<Cinema> _repository;// = new Repository<Brand>();

        public CinemaController(IRepository<Cinema> repository)
        {
            _repository = repository;
        }

        public IActionResult Index(string? query, int page = 1, int size = 4)
        {
            //var cinemas = _db.Cinemas.AsQueryable();
            var cinemas = _repository.Get();

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
        public async Task<IActionResult> Create(Cinema cinema, IFormFile Img, CancellationToken ct = default)
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

            //_db.Cinemas.Add(cinema);
            //_db.SaveChanges();

            await _repository.CreateAsync(cinema, ct);
            await _repository.CommitAsync(ct);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            //var cinema = _db.Cinemas.AsNoTracking().FirstOrDefault(e=>e.Id == Id);
            var cinema = _repository.GetOne(e => e.Id == id, tracked: false);



            if (cinema is null) return RedirectToAction(nameof(HomeController.NotFoundPage), ControllerConstants.HOME_CONTROLLER);

            return View(cinema);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Cinema cinema, IFormFile Img, CancellationToken ct = default)
        {
            //var cinemaDb = _db.Cinemas.AsNoTracking().FirstOrDefault(e => e.Id == Cinema.Id);

            var cinemaDb = _repository.GetOne(e => e.Id == cinema.Id, tracked: false);

            if (cinemaDb is null) return NotFound();

            if (Img is not null && Img.Length > 0)
            {
                var fileName = $"{Guid.NewGuid().ToString()}-{DateTime.Now.ToString("dd-MM-yy")}{Path.GetExtension(Img.FileName)}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgs", "cinemas", fileName);

                using (var Stream = System.IO.File.Create(filePath))
                {
                    Img.CopyTo(Stream);
                }
                //Cinema.Img = fileName;

                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgs", "cinemas", cinemaDb.Img);
                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);

                cinema.Img = fileName;
            }
            else
                cinema.Img = cinemaDb.Img;

            //_db.Cinemas.Update(Cinema);
            //_db.SaveChanges();

            _repository.Update(cinema);
            await _repository.CommitAsync(ct);

            TempData[NotificationConstants.SUCCESS_NOTIFICATION] = "Update Cinema Successfully";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            //var cinema = _db.Cinemas.FirstOrDefault(e => e.Id == id);

            var cinema = _repository.GetOne(e => e.Id == id);

            if (cinema is null) return RedirectToAction(nameof(HomeController.NotFoundPage), ControllerConstants.HOME_CONTROLLER);

            //_db.Cinemas.Remove(cinema);
            //_db.SaveChanges();

            _repository.Update(cinema);
            await _repository.CommitAsync(ct);

            TempData[NotificationConstants.SUCCESS_NOTIFICATION] = "Delete Cinema Successfully";

            return RedirectToAction(nameof(Index));

        }
    }
}
