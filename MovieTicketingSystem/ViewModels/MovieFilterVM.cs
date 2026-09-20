namespace MovieTicketingSystem.ViewModels
{
    public record MovieFilterVM(string? name,int? categoryId, int? cinemaId, decimal? minPrice, decimal? maxPrice);
}
