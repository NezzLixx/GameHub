using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameHub.Infrastructure.Data;
using GameHub.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace GameHub.Infrastructure.Repositories;

// Клас прямої взаємодії таблиці Ігор (Games) з базою даних SQLite через EF Core
public class GameRepository
{
    private readonly GameHubDbContext _context;

    public GameRepository(GameHubDbContext context)
    {
        _context = context;
    }

    // Асинхронний метод пагінації, фільтрації та сортування ігор
    public async Task<(List<Game> Items, int TotalCount)> GetPagedGamesAsync(
        string? searchItem = null,
        int? genreId = null,
        string? sortBy = null,
        int pageNumber = 1,
        int pageSize = 12)
    {
        // Запит з підвантаженням зв'язаних таблиць (Eager Loading)
        IQueryable<Game> query = _context.Games
            .Include(g => g.Developer)
            .Include(g => g.Genre)
            .Include(g => g.Images);

        // Фільтр пошуку по назві (регістронезалежний)
        if (!string.IsNullOrWhiteSpace(searchItem))
        {
            string lowerSearch = searchItem.ToLower();
            query = query.Where(g => g.Title.ToLower().Contains(lowerSearch));
        }

        // Фільтр по конкретному жанру
        if (genreId.HasValue)
        {
            query = query.Where(g => g.GenreId == genreId.Value);
        }

        // Сортування (switch-вираз)
        query = sortBy switch
        {
            "Title" => query.OrderBy(g => g.Title),
            "TitleDesc" => query.OrderByDescending(g => g.Title),
            _ => query.OrderBy(g => g.Id) // По дефолту сортує за ID
        };
        
        // Отримуємо загальну кількість записів, що підійшли під фільтри
        int totalCount = await query.CountAsync();

        // Пагінація: пропускаємо старі сторінки (.Skip) і беремо необхідну кількість (.Take)
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return (items, totalCount);
    }
    
    // Отримання детальної інформації про гру за ID разом з відгуками
    public async Task<Game?> GetByIdAsync(int id)
    {
        return await _context.Games
            .Include(g => g.Developer)
            .Include(g => g.Genre)
            .Include(g => g.Images)
            .Include(g => g.Reviews)
                .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    // Додавання нової гри в БД
    public async Task AddAsync(Game game)
    {
        await _context.Games.AddAsync(game);
        await _context.SaveChangesAsync();
    }

    // Видалення гри за ID
    public async Task DeleteAsync(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game != null)
        {
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
        }
    }
}