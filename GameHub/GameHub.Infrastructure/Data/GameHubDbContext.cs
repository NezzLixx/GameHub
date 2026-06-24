using GameHub.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameHub.Infrastructure.Data;

public class GameHubDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Developer> Developers { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<GameImage> GamesImages { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=gamehub.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        
        modelBuilder.Entity<Review>().HasIndex(r => new { r.UserId, r.GameId }).IsUnique();
        
        modelBuilder.Entity<Game>()
            .HasMany (g => g.Images)
            .WithOne(i => i.Game)
            .HasForeignKey(i => i.GameId)
            .OnDelete(DeleteBehavior.Cascade);  
    }
}