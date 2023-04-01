using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using MiniMvvm;

namespace ScratchApp.ViewModels;

public class AppViewModel : ViewModelBase
{
    public AppViewModel()
    {
        ViewTitle = "Scratch App";
        ExitCommand = MiniCommand.Create(() =>
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
            {
                lifetime.Shutdown();
            }
        });

    }

    public MiniCommand ExitCommand { get; }
}
