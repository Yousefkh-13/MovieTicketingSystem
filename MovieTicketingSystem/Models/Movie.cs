using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MovieTicketingSystem.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string MainImg { get; set; } = string.Empty;
        public ICollection<MovieSubImg> MovieSubImgs { get; set; } = new List<MovieSubImg>();
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; } = null!;
        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
    }
}
