namespace MovieTicketingSystem.DataAccess.EntityConfigurations
{
    public class MovieActorEntityTypeConfiguration : IEntityTypeConfiguration<MovieActor>
    {
        public void Configure(EntityTypeBuilder<MovieActor> builder)
        {
            builder.HasKey(e => new { e.MovieId, e.ActorId });

            // One-to-many relationship between Movie and MovieActors  
            builder.HasOne(e => e.Movie).WithMany(e => e.MovieActors).HasForeignKey(e => e.MovieId).OnDelete(DeleteBehavior.Cascade);

            // One-to-many relationship between Actor and MovieActors  
            builder.HasOne(e => e.Actor).WithMany(e => e.MovieActors).HasForeignKey(e => e.ActorId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
