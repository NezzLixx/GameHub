using System.Threading.Tasks;
using GameHub.Infrastructure.Entity;
using GameHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameHub.Infrastructure.Repositories;

// Клас взаємодії з таблицею Користувачів (Users) в БД
public class UserRepository
{
    private readonly GameHubDbContext _context;
    
    public UserRepository(GameHubDbContext context)
    {
        _context = context;
    }

    // Пошук користувача за Email (для авторизації та перевірки реєстрації)
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    // Пошук за нікнеймом
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
    
    // Збереження нового юзера в базу даних
    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}