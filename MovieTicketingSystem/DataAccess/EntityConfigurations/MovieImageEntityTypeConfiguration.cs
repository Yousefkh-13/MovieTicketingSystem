
namespace MovieTicketingSystem.DataAccess.EntityConfigurations
{
    public class MovieImageEntityTypeConfiguration : IEntityTypeConfiguration<MovieSubImg>
    {
        public void Configure(EntityTypeBuilder<MovieSubImg> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.ImageUrl).IsRequired().HasMaxLength(500);
            //// One-to-many relationship between Movie and SubImgs  
            //builder.HasOne(e => e.Movie).WithMany(e => e.ImageUrl).HasForeignKey(e => e.MovieId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
