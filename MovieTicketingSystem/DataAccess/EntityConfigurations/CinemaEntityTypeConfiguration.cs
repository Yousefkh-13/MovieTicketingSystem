namespace MovieTicketingSystem.DataAccess.EntityConfigurations
{
    public class CinemaEntityTypeConfiguration : IEntityTypeConfiguration<Cinema>
    {
        public void Configure(EntityTypeBuilder<Cinema> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Status).HasDefaultValue(true);
            builder.Property(e => e.Img).HasMaxLength(500);
            // One-to-many relationship between Cinema and Movie
            builder.HasMany(c => c.Movies).WithOne(m => m.Cinema).HasForeignKey(m => m.CinemaId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
