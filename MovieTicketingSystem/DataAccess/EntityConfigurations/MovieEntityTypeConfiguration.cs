namespace MovieTicketingSystem.DataAccess.EntityConfigurations
{
    public class MovieEntityTypeConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(200);
            builder.Property(e => e.Status).HasDefaultValue(true);
            builder.Property(e => e.Price).HasPrecision(10, 2);
            builder.Property(e => e.MainImg).IsRequired().HasMaxLength(500);
            builder.Property(e => e.StartDateTime).IsRequired();
            builder.Property(e => e.EndDateTime).IsRequired();
            // One-to-many relationship between Movie and SubImgs  
            //builder.HasMany(m => m.SubImgs).WithOne(i => i.Movie).HasForeignKey(i => i.MovieId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
