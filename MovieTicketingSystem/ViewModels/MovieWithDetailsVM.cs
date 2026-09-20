namespace MovieTicketingSystem.ViewModels
{
    public class MovieWithDetailsVM
    {
        public Movie? Movie { get; set; } = null!;
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<Cinema> Cinemas { get; set; } = new List<Cinema>();
        public IEnumerable<MovieSubImg> MovieSubImgs { get; set; } = new List<MovieSubImg>();
        public IEnumerable<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
        public IEnumerable<Actor> Actors { get; set; } = new List<Actor>();
    }
}
