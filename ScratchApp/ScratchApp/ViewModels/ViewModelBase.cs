using Avalonia.Controls;

namespace ScratchApp.ViewModels;

public class ViewModelBase : NotificationBase
{
    public ViewModelBase()
    {
        IsInDesignMode = Design.IsDesignMode;
    }

    public bool IsInDesignMode { get; }

    private string? _viewTitle;
    public string? ViewTitle
    {
        get { return _viewTitle; }
        set
        {
            _viewTitle = value;
            OnPropertyChanged(nameof(ViewTitle));
        }
    }
}