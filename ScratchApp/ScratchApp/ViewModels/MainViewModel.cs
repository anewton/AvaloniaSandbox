using Avalonia.Controls.ApplicationLifetimes;
using ScratchApp.Models;

namespace ScratchApp.ViewModels;

public class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        ViewTitle = "Main Page";

        Thing = new EditableThing() { SomeNumber = 42, SomeText = "The meaning of life?" };
    }

    public MainViewModel(IControlledApplicationLifetime desktopLifetime) : this()
    {
        desktopLifetime.Exit += DesktopLifetime_Exit;
    }

    private void DesktopLifetime_Exit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        // do exit app cleanup here if needed
    }

    public EditableThing Thing
    {
        get => _thing;
        set => SetProperty(ref _thing, value, nameof(Thing));
    }
    private EditableThing _thing;

}