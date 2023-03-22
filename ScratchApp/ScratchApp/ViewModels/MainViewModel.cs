using Avalonia.Controls.ApplicationLifetimes;

namespace ScratchApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        ViewTitle = "Main Page";
    }

    public MainViewModel(IControlledApplicationLifetime desktopLifetime) : this()
    {
        desktopLifetime.Exit += DesktopLifetime_Exit; 
    }

    private void DesktopLifetime_Exit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        // do exit app cleanup here if needed
    }
}