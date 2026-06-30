using System;

namespace GameHub.Infrastructure.Entity;

public class Review
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int GameId { get; set; }
    public Game Game { get; set; } = null!;
    
    public string ReviewText { get; set; } = string.Empty;
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public byte[]? AttachedImageData { get; set; } 
}