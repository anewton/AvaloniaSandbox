using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using ScratchApp.ViewModels;
using ScratchApp.Views;

namespace ScratchApp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ViewLocator>();
        serviceCollection.AddScoped<MainViewModel>();
        serviceCollection.AddSingleton<MainWindow>((serviceProvider) => new MainWindow() { DataContext = serviceProvider.GetService<ViewLocator>()?.MainVM });
        var serviceProvider = serviceCollection.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = serviceProvider.GetService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}