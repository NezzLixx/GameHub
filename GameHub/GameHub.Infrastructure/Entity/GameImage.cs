namespace GameHub.Infrastructure.Entity;

public class GameImage
{
    public int Id { get; set; }
    public int GameId { get; set; }
    public Game Game { get; set; } = null!;
    
    public string ImagePath { get; set; } = string.Empty;
}