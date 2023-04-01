using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using ScratchApp.ViewModels;
using ScratchApp.Views;

namespace ScratchApp;

public partial class App : Application
{
    private ServiceProvider? _serviceProvider;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<AppViewModel>();
        serviceCollection.AddScoped<MainViewModel>();

        // Give a constructor in MainWindow the IServiceProvider so it can set DataContext in the constructor
        serviceCollection.AddSingleton((serviceProvider) => new MainWindow(serviceProvider.GetRequiredService<MainViewModel>()));
        _serviceProvider = serviceCollection.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Set the MainWindow
            desktop.MainWindow = _serviceProvider.GetService<MainWindow>();
        }

        // Set App DataContext
        DataContext = _serviceProvider.GetService<AppViewModel>();

        base.OnFrameworkInitializationCompleted();
    }
}