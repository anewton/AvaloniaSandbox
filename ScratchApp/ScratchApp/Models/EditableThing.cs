using ScratchApp.ViewModels;
using System.ComponentModel;

namespace ScratchApp.Models;

public class EditableThing : NotificationBase
{
    [DisplayName("Some Number")]
    public int SomeNumber
    {
        get { return _someNumber; }
        set { SetProperty(ref _someNumber, value); }
    }
    private int _someNumber;

    [DisplayName("Some Text")]
    public string SomeText
    {
        get { return _someText; }
        set { SetProperty(ref _someText, value); }
    }
    private string _someText;
}
