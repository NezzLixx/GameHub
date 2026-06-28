namespace GameHub.Infrastructure.Entity;

public class Game
{
    public int Id { get; set; }
    public string Title { get; set; } =  string.Empty;
    public string Description { get; set; } =  string.Empty;
    
    public int DeveloperId { get; set; }
    public Developer Developer { get; set; } = null!;
    
    public int GenreId { get; set; }
    public Genre Genre { get; set; } = null!;

    public List<GameImage> Images { get; set; } = new();
    
    public List<Review> Reviews { get; set; } = new();
}