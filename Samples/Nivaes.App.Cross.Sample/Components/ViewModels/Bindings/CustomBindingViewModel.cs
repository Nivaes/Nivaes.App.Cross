using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class CustomBindingViewModel
    : CrossNavigationViewModel
{
    private ICrossAsyncCommand? _closeCommand;

    private int _counter = 2;

    private DateTime _date = DateTime.Now;

    private string _hello = "Hello MvvmCross";

    public CustomBindingViewModel(ILogger<CustomBindingViewModel> logger)
        : base(logger)
    {
    }

    public string Hello
    {
        get => _hello;
        set => SetProperty(ref _hello, value);
    }

    public ICrossAsyncCommand CloseCommand =>
        _closeCommand ??= new CrossAsyncCommand(() => NavigationService.Close(this));

    public int Counter
    {
        get => _counter;
        set => SetProperty(ref _counter, value);
    }

    public DateTime Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }
}
