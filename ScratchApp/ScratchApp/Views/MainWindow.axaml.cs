using Avalonia.Controls;
using ScratchApp.ViewModels;

namespace ScratchApp.Views;

public partial class MainWindow : Window
{
    private readonly MainViewModel? _mainViewModel;

    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(MainViewModel mainViewModel) : this()
    {
        _mainViewModel = mainViewModel;
        DataContext = _mainViewModel;
    }
}