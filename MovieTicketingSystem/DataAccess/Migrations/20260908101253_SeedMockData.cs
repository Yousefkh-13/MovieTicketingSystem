using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketingSystem.DataAccess.Migrations
{
    public partial class SeedMockData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO Categories (Name, Description, Status) VALUES 
                (N'Action', N'High energy, stunts, and chase scenes', 1),
                (N'Comedy', N'Humorous and family-friendly entertainment', 1),
                (N'Drama', N'Character-driven narrative and emotional themes', 1),
                (N'Sci-Fi', N'Futuristic concepts, space exploration, and technology', 1),
                (N'Horror', N'Scary, suspenseful, and thrilling experiences', 1),
                (N'Animation', N'Animated movies for kids and adults', 1);
            ");

            migrationBuilder.Sql(@"
                INSERT INTO Cinemas (Name, Img, Status) VALUES 
                (N'Grand Cinema - Mall of Arabia', N'/images/cinemas/grand-mall-arabia.jpg', 1),
                (N'Vox Cinemas - City Centre Almaza', N'/images/cinemas/vox-almaza.jpg', 1),
                (N'Galaxy Cinema - Cairo Festival City', N'/images/cinemas/galaxy-cfc.jpg', 1),
                (N'IMAX - Arkan Plaza', N'/images/cinemas/imax-arkan.jpg', 1),
                (N'Renaissance - San Stefano Alex', N'/images/cinemas/renaissance-alex.jpg', 1);
            ");

            migrationBuilder.Sql(@"
                INSERT INTO Actors (Name, Img) VALUES 
                (N'Robert Downey Jr.', N'/images/actors/rdj.jpg'),
                (N'Cillian Murphy', N'/images/actors/cillian.jpg'),
                (N'Leonardo DiCaprio', N'/images/actors/dicaprio.jpg'),
                (N'Tom Hardy', N'/images/actors/tom-hardy.jpg'),
                (N'Christian Bale', N'/images/actors/bale.jpg'),
                (N'Margot Robbie', N'/images/actors/margot.jpg'),
                (N'Brad Pitt', N'/images/actors/brad-pitt.jpg'),
                (N'Ryan Gosling', N'/images/actors/gosling.jpg');
            ");

            migrationBuilder.Sql(@"
                INSERT INTO Movies (Name, Description, Price, Status, StartDateTime, EndDateTime, MainImg, CategoryId, CinemaId) VALUES 
                (N'Oppenheimer', N'The story of American scientist J. Robert Oppenheimer and his role in the Manhattan Project.', 180.00, 1, GETDATE(), DATEADD(month, 2, GETDATE()), N'/images/movies/oppenheimer.jpg', 3, 4),
                (N'Inception', N'A thief who steals corporate secrets through dream-sharing technology is given the inverse task.', 150.00, 1, GETDATE(), DATEADD(month, 1, GETDATE()), N'/images/movies/inception.jpg', 4, 1),
                (N'The Dark Knight', N'When the menace known as the Joker wreaks havoc and chaos on Gotham, Batman must accept his test.', 160.00, 1, GETDATE(), DATEADD(month, 2, GETDATE()), N'/images/movies/dark-knight.jpg', 1, 2),
                (N'Barbie', N'Barbie and Ken are having the time of their lives in the colorful Barbie Land before heading to the real world.', 140.00, 1, GETDATE(), DATEADD(month, 1, GETDATE()), N'/images/movies/barbie.jpg', 2, 3),
                (N'Once Upon a Time in Hollywood', N'A faded television actor and his stunt double strive to achieve fame and success in 1969 Los Angeles.', 130.00, 1, GETDATE(), DATEADD(month, 1, GETDATE()), N'/images/movies/hollywood.jpg', 3, 5);
            ");

            migrationBuilder.Sql(@"
                INSERT INTO MovieImages (ImageUrl, MovieId) VALUES 
                -- Oppenheimer
                (N'/images/movies/sub/oppenheimer-1.jpg', 1),
                (N'/images/movies/sub/oppenheimer-2.jpg', 1),
                (N'/images/movies/sub/oppenheimer-3.jpg', 1),
                -- Inception
                (N'/images/movies/sub/inception-1.jpg', 2),
                (N'/images/movies/sub/inception-2.jpg', 2),
                -- Dark Knight
                (N'/images/movies/sub/darkknight-1.jpg', 3),
                (N'/images/movies/sub/darkknight-2.jpg', 3),
                -- Barbie
                (N'/images/movies/sub/barbie-1.jpg', 4),
                (N'/images/movies/sub/barbie-2.jpg', 4),
                -- Once Upon a Time in Hollywood
                (N'/images/movies/sub/hollywood-1.jpg', 5);
            ");

            migrationBuilder.Sql(@"
                INSERT INTO MovieActors (MovieId, ActorId) VALUES 
                -- Oppenheimer (Robert Downey Jr., Cillian Murphy)
                (1, 1),
                (1, 2),
                -- Inception (Leonardo DiCaprio, Tom Hardy, Cillian Murphy)
                (2, 3),
                (2, 4),
                (2, 2),
                -- The Dark Knight (Christian Bale, Cillian Murphy)
                (3, 5),
                (3, 2),
                -- Barbie (Margot Robbie, Ryan Gosling)
                (4, 6),
                (4, 8),
                -- Once Upon a Time in Hollywood (Leonardo DiCaprio, Brad Pitt, Margot Robbie)
                (5, 3),
                (5, 7),
                (5, 6);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Truncate table MovieActors;");
            migrationBuilder.Sql("Truncate table MovieImages;");
            migrationBuilder.Sql("Truncate table Movies;");
            migrationBuilder.Sql("Truncate table Actors;");
            migrationBuilder.Sql("Truncate table Cinemas;");
            migrationBuilder.Sql("Truncate table Categories;");
        }
    }
}
