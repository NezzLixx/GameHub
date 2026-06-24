using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GameHub.Infrastructure.Data;
using GameHub.Infrastructure.Entities;
using GameHub.Infrastructure.Repositories;
using GameHub.UI.ViewModels;
using GameHub.UI.Views;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GameHub.UI;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };

            try
            {
                using var context = new GameHubDbContext();
                
                context.Database.Migrate(); 

                var gameRepo = new GameRepository(context);

                var (items, total) = await gameRepo.GetPagedGamesAsync(pageSize: 1);
                if (total == 0)
                {
                    var testGenre = new Genre { Name = "RPG" };
                    var testDev = new Developer { Name = "CD Projekt RED", Description = "Best studio" };

                    var testGame = new Game
                    {
                        Title = "The Witcher 3",
                        Description = "Masterpiece",
                        Developer = testDev,
                        Genre = testGenre
                    };

                    await gameRepo.AddAsync(testGame);
                    Debug.WriteLine("=== ТЕСТ: Гру успішно додано в базу! ===");
                }

                var (gamesAfterInsert, count) = await gameRepo.GetPagedGamesAsync();
                var firstGame = gamesAfterInsert.FirstOrDefault();
                
                if (firstGame != null)
                {
                    Debug.WriteLine($"=== ТЕСТ РЕПОЗИТОРІЮ: Знайдено гру '{firstGame.Title}' від розробника {firstGame.Developer?.Name}! ===");
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine($"=== ПОМИЛКА ТЕСТУ: {ex.Message} ===");
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
}