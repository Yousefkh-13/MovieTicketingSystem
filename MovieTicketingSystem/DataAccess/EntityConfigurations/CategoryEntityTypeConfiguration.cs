namespace MovieTicketingSystem.DataAccess.EntityConfigurations
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(200);
            builder.Property(e => e.Status).HasDefaultValue(true);
            // One-to-many relationship between Category and Movie
            builder.HasMany(c => c.Movies).WithOne(m => m.Category).HasForeignKey(m => m.CategoryId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
