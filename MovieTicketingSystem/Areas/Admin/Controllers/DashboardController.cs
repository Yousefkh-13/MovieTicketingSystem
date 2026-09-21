namespace MovieTicketingSystem.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        //private readonly ApplicationDbContext _db = new();
        private readonly IRepository<DashboardController> _dashboardrepository;// = new Repository<Category>();
        private readonly IRepository<Movie> _movieRepository;
        private readonly IRepository<Category> _categoryRepository;// = new Repository<Category>();
        private readonly IRepository<Cinema> _cinemaRepository;// = new Repository<Cinema>();
        private readonly IRepository<Actor> _actorRepository;// = new Repository<Actor>();

        public DashboardController(
            IRepository<DashboardController> dashboardrepository,
            IRepository<Movie> movieRepository,
            IRepository<Category> categoryRepository,
            IRepository<Cinema> cinemaRepository,
            IRepository<Actor> actorRepository
            )
        {
            _dashboardrepository = dashboardrepository;
            _movieRepository = movieRepository;
            _categoryRepository = categoryRepository;
            _cinemaRepository = cinemaRepository;
            _actorRepository = actorRepository;

        }

        //public IActionResult Index()
        //{
        //    var dashboardVM = new DashboardVM
        //    {
        //        TotalCinemas = _db.Cinemas.Count(),
        //        TotalMovies = _db.Movies.Count(),
        //        TotalActors = _db.Actors.Count(),
        //        TotalCategories = _db.Categories.Count()
        //    }
        //    ;

        //    return View(model);
        //}
    }
}
