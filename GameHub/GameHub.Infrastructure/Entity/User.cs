namespace GameHub.Infrastructure.Entity;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } =  string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User";

    public List<Review> Reviews { get; set; } = new();
}