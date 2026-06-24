using System;
using System.Collections.Generic;

namespace GameHub.Infrastructure.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } =  string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User";

    public List<Review> Reviews { get; set; } = new();
}

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<Game> Games { get; set; } = new();
}

public class Developer
{
    public int Id { get; set; }
    public string Name { get; set; } =  string.Empty;
    public string Description { get; set; } =  string.Empty;
    
    public List<Game> Games { get; set; } = new();
}

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

public class GameImage
{
    public int Id { get; set; }
    public int GameId { get; set; }
    public Game Game { get; set; } = null!;
    
    public string ImagePath { get; set; } = string.Empty;
}

public class Review
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int GameId { get; set; }
    public Game Game { get; set; } = null!;
    
    public string ReviewText { get; set; } = string.Empty;
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; } =  DateTime.UtcNow;
}
