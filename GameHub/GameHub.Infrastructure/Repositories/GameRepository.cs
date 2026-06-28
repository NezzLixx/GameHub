using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameHub.Infrastructure.Data;
using GameHub.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace GameHub.Infrastructure.Repositories;

public class GameRepository
{
    private readonly GameHubDbContext _context;

    public GameRepository(GameHubDbContext context)
    {
        _context = context;
    }

    public async Task<(List<Game> Items, int TotalCount)> GetPagedGamesAsync(
        string? searchItem = null,
        int? genreId = null,
        string? sortBy = null,
        int pageNumber = 1,
        int pageSize = 6)
    {
        IQueryable<Game> query = _context.Games
            .Include(g => g.Developer)
            .Include(g => g.Genre)
            .Include(g => g.Images);

        if (!string.IsNullOrWhiteSpace(searchItem))
        {
            string lowerSearch = searchItem.ToLower();
            query = query.Where(g => g.Title.ToLower().Contains(lowerSearch));
        }

        if (genreId.HasValue)
        {
            query = query.Where(g => g.GenreId == genreId.Value);
        }

        query = sortBy switch
        {
            "Title" => query.OrderBy(g => g.Title),
            "TitleDesc" => query.OrderByDescending(g => g.Title),
            _ => query.OrderBy(g => g.Id)
        };
        
        int totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return (items, totalCount);
    }
    
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

    public async Task AddAsync(Game game)
    {
        await _context.Games.AddAsync(game);
        await _context.SaveChangesAsync();
    }

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