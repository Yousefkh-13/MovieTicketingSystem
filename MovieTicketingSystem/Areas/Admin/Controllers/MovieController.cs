using MovieTicketingSystem.ViewModels;

namespace MovieTicketingSystem.Areas.Admin.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _db = new();
        IFileUpload fileUpload = new FileUpload();

        public IActionResult Index(MovieFilterVM movieFilterVM, int page = 1, int size = 4)
        {
            var movies = _db.Movies.Include(e => e.Category).Include(e => e.Cinema).AsQueryable();

            // Filtering
            if (movieFilterVM.name != null)
                movies = movies.Where(e => e.Name.ToLower().Contains(movieFilterVM.name.ToLower()));

            if (movieFilterVM.minPrice != null)
                movies = movies.Where(e => e.Price >= movieFilterVM.minPrice);

            if (movieFilterVM.maxPrice != null)
                movies = movies.Where(e => e.Price <= movieFilterVM.maxPrice);

            if (movieFilterVM.categoryId != null)
                movies = movies.Where(e => e.CategoryId == movieFilterVM.categoryId);

            if (movieFilterVM.cinemaId != null)
                movies = movies.Where(e => e.CinemaId == movieFilterVM.cinemaId);


            // Pagination
            var totalPages = Math.Ceiling(movies.Count() / (double)size);
            movies = movies.Skip((page - 1) * size).Take(size);

            var categories = _db.Categories.AsQueryable();
            var cinemas = _db.Cinemas.AsQueryable();

            return View(new MovieWithFilterVM()
            {
                Movies = movies,
                Categories = categories,
                Cinemas = cinemas,
                TotalPages = totalPages,
                CurrentPage = page,
                Name = movieFilterVM.name ?? "",
                MinPrice = movieFilterVM.minPrice,
                MaxPrice = movieFilterVM.maxPrice,
                CategoryId = movieFilterVM.categoryId,
                CinemaId = movieFilterVM.cinemaId,
                
            });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var categories = _db.Categories.AsQueryable();
            var cinemas = _db.Cinemas.AsQueryable();
            var actors = _db.Actors.AsQueryable();
            
            return View(new MovieWithDetailsVM()
            {
                Categories = categories,
                Cinemas = cinemas,
                Actors = actors
            });
        }
        [HttpPost]
        public IActionResult Create(Movie movie, IFormFile mainImg, List<IFormFile> subImgs, List<string> actors)
        {
            if (mainImg is not null && mainImg.Length > 0)
            {
                var fileName = fileUpload.GenerateFileName(mainImg.FileName);
                if (fileName is null) return BadRequest();

                var filePath = fileUpload.GenerateFullPath(FileType.Img, "movies", fileName);
                if (filePath is null) return BadRequest();

                fileUpload.UploadFileLocally(filePath, mainImg);
                movie.MainImg = fileName;
            }

            _db.Movies.Add(movie);
            _db.SaveChanges();

            if (actors is not null && actors.Count > 0)
            {
                foreach (var actorIdStr in actors)
                {
                    if (int.TryParse(actorIdStr, out int actorId))
                    {
                        var movieActor = new MovieActor
                        {
                            MovieId = movie.Id,
                            ActorId = actorId
                        };
                        _db.MovieActors.Add(movieActor);
                    }
                }
            }

            if (subImgs.Any())
            {
                foreach (var item in subImgs)
                {
                    var fileName = fileUpload.GenerateFileName(item.FileName);
                    if (fileName is null) return BadRequest();


                    var filePath = fileUpload.GenerateFullPath(FileType.Img, "movies\\sub-imgs", fileName);
                    if (filePath is null) return BadRequest();

                    fileUpload.UploadFileLocally(filePath, item);

                    _db.MovieSubImgs.Add(new MovieSubImg()
                    {
                        ImageUrl = fileName,
                        MovieId = movie.Id,
                    });
                }
            }
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var movie = _db.Movies.AsNoTracking().FirstOrDefault(e => e.Id == id);

            if (movie is null) return NotFound();

            var categories = _db.Categories.AsQueryable();
            var cinemas = _db.Cinemas.AsQueryable();
            var actors = _db.Actors.AsQueryable();
            var movieSubImgs = _db.MovieSubImgs.Where(e => e.MovieId == movie.Id);

            return View(new MovieWithDetailsVM()
            {
                Movie = movie ?? new(),
                MovieSubImgs = movieSubImgs,
                Categories = categories,
                Cinemas = cinemas, 
                Actors = actors
            });
        }

        [HttpPost]
        public IActionResult Update(Movie movie, IFormFile mainImg, List<IFormFile> subImgs, List<string> actors)
        {
            var movieInDb = _db.Movies.AsNoTracking().FirstOrDefault(e => e.Id == movie.Id);
            if (movieInDb is null) return NotFound();

            if (mainImg is not null && mainImg.Length > 0)
            {
                var fileName = fileUpload.GenerateFileName(mainImg.FileName);
                if (fileName is null) return BadRequest();

                var filePath = fileUpload.GenerateFullPath(FileType.Img, "movies", fileName);
                if (filePath is null) return BadRequest();

                fileUpload.UploadFileLocally(filePath, mainImg);

                var oldFilePath = fileUpload.GenerateFullPath(FileType.Img, "movies", movieInDb.MainImg);
                if (oldFilePath is null) return BadRequest();

                fileUpload.DeleteFileLocally(oldFilePath);

                movie.MainImg = fileName;
            }
            else movie.MainImg = movieInDb.MainImg;

            _db.Movies.Update(movie);
            _db.SaveChanges();

            if (subImgs.Any())
            {
                var oldImgs = _db.MovieSubImgs.Where(e => e.MovieId == movie.Id);

                foreach (var item in oldImgs)
                {
                    var oldFilePath = fileUpload.GenerateFullPath(FileType.Img, "movies\\sub-imgs", item.ImageUrl);
                    if (oldFilePath is null) return BadRequest();

                    fileUpload.DeleteFileLocally(oldFilePath);
                }

                _db.MovieSubImgs.RemoveRange(oldImgs);

                foreach (var item in subImgs)
                {
                    var fileName = fileUpload.GenerateFileName(item.FileName);
                    if (fileName is null) return BadRequest();

                    var filePath = fileUpload.GenerateFullPath(FileType.Img, "movies\\sub-imgs", fileName);
                    if (filePath is null) return BadRequest();

                    fileUpload.UploadFileLocally(filePath, item);

                    _db.MovieSubImgs.Add(new MovieSubImg()
                    {
                        ImageUrl = fileName,
                        MovieId = movie.Id
                    });
                }    
                _db.SaveChanges();
            }

            if (actors is not null)
            {
                var oldMovieActors = _db.MovieActors.Where(e => e.MovieId == movie.Id);
                _db.MovieActors.RemoveRange(oldMovieActors);

                foreach (var actorIdStr in actors)
                {
                    if (int.TryParse(actorIdStr, out int actorId))
                    {
                        _db.MovieActors.Add(new MovieActor()
                        {
                            MovieId = movie.Id,
                            ActorId = actorId
                        });
                    }
                }
                _db.SaveChanges();
            }
           return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var movie = _db.Movies
                .Where(e => e.Id == id)
                //.Include(e=>e.MainImg)
                .Include(e => e.Cinema)
                .Include(e => e.Category)
                .Include(e => e.MovieSubImgs)
                .Include(e => e.MovieActors)
                .ThenInclude(e => e.Actor)
                .FirstOrDefault();

            if (movie is null) return NotFound();

            return View(movie);
        }
    }
}


