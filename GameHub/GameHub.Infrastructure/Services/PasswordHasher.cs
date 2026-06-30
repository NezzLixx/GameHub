using System.Security.Cryptography;
using System.Text;

namespace GameHub.Infrastructure.Services;

public static class PasswordHasher
{
    // Метод, який перетворює звичайний текст на незворотний SHA256 хеш
    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        
        var builder = new StringBuilder();
        foreach (var b in bytes)
        {
            builder.Append(b.ToString("x2")); // Перетворюємо в шістнадцятковий рядок
        }
        return builder.ToString();
    }
}