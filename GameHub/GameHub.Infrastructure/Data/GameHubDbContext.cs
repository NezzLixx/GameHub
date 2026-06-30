using GameHub.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace GameHub.Infrastructure.Data;

// Головний сервіс конфігурації Entity Framework Core та мапінгу таблиць
public class GameHubDbContext : DbContext
{
    // Реєстрація таблиць бази даних
    public DbSet<User> Users { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Developer> Developers { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<GameImage> GamesImages { get; set; }
    public DbSet<Review> Reviews { get; set; }

    // Конфігурація підключення до файлу SQLite
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=gamehub.db");
    }

    // Налаштування обмежень та зв'язків між таблицями (Fluent API)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Унікальні індекси (захист від дублікатів нікнеймів та пошт)
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        
        // Один користувач — один відгук на конкретну гру
        modelBuilder.Entity<Review>().HasIndex(r => new { r.UserId, r.GameId }).IsUnique();
        
        // Зв'язок Один-до-Багатьох (Гра -> Картинки) з каскадним видаленням
        modelBuilder.Entity<Game>()
            .HasMany (g => g.Images)
            .WithOne(i => i.Game)
            .HasForeignKey(i => i.GameId)
            .OnDelete(DeleteBehavior.Cascade);  
    }
}