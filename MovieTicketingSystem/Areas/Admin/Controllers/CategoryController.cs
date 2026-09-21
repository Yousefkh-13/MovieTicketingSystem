using MovieTicketingSystem.Repositories.IRepositories;

namespace MovieTicketingSystem.Areas.Admin.Controllers
{
    [Area(AreaConstants.ADMIN_AREA)]

    public class CategoryController : Controller
    {


        //private readonly ApplicationDbContext _db = new();
        private readonly IRepository<Category> _repository;// = new Repository<Category>();


        public CategoryController(IRepository<Category> repository)
        {
            _repository = repository;
        }


        public IActionResult Index(string? query, int page = 1, int size = 4)
        {
            var categories = _repository.Get();

            if (query is not null)
                categories = categories.Where(e => e.Name.ToLower().Contains(query.ToLower()));

            var totalPages = Math.Ceiling(categories.Count() / (double)size);

            categories = categories.Skip((page - 1) * size).Take(size);

            return View(new CategoryWithFilterVM
            {
                Categories = categories,
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
        public async Task<IActionResult> Create(Category category, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return View(category);

            await _repository.CreateAsync(category, ct);
            await _repository.CommitAsync(ct);

            TempData[NotificationConstants.SUCCESS_NOTIFICATION] = "Create Category Successfully";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            //var category = _db.Categories.AsNoTracking().FirstOrDefault(e=>e.Id == Id);
            var category = _repository.GetOne(e => e.Id == id, tracked: false);


            if (category is null) return RedirectToAction(nameof(HomeController.NotFoundPage), ControllerConstants.HOME_CONTROLLER);

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Category category, CancellationToken ct = default)
        {
            //_db.Categories.Add(new Category()
            //{
            //    Name = name,
            //    Description = description,
            //    Status = status
            //});

            //_db.Categories.Update(category);
            //_db.SaveChanges();

            _repository.Update(category);
            await _repository.CommitAsync(ct);


            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        {
            //var category = _db.Categories.FirstOrDefault(e => e.Id == id);
            var category = _repository.GetOne(e => e.Id == id);

            if (category is null) 
                return RedirectToAction(nameof(HomeController.NotFoundPage), ControllerConstants.HOME_CONTROLLER);


            //_db.Categories.Remove(category);
            //_db.SaveChanges();

            _repository.Delete(category);
            await _repository.CommitAsync(ct);

            TempData[NotificationConstants.SUCCESS_NOTIFICATION] = "Delete Category Successfully";


            return RedirectToAction(nameof(Index));

        }
    }
}
