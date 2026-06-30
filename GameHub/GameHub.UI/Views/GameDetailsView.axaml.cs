using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using GameHub.UI.ViewModels;
using System.Linq;

namespace GameHub.UI.Views;

public partial class GameDetailsView : UserControl
{
    public GameDetailsView()
    {
        InitializeComponent();
    }

    private async void AttachFileBtn_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is GameDetailsViewModel vm)
        {
            // Отримуємо доступ до вікна для виклику StorageProvider
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null) return;

            // Відкриваємо системне діалогове вікно вибору файлів
            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Виберіть зображення або GIF для відгуку",
                AllowMultiple = false,
                FileTypeFilter = new[] { FilePickerFileTypes.ImageAll } // Тільки картинки/гіфки
            });

            if (files.Count > 0)
            {
                // Передаємо шлях до локального файлу у ViewModel
                var localPath = files.First().Path.LocalPath;
                vm.AttachFile(localPath);
            }
        }
    }
}