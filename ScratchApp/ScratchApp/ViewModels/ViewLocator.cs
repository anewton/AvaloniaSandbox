namespace ScratchApp.ViewModels;

public class ViewLocator
{
    public ViewLocator(MainViewModel mainVM)
    {
        _mainVM = mainVM;
    }

    private readonly MainViewModel _mainVM;
    public MainViewModel MainVM => _mainVM;
}
