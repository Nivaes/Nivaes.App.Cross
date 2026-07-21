using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class DictionaryBindingViewModel : BaseViewModel
{
    int _value = 0;
    public int Value
    {
        get => _value;
        set => SetProperty(ref _value, value);
    }

    ICrossAsyncCommand _closeCommand;
    public ICrossAsyncCommand CloseCommand =>
        _closeCommand ?? (_closeCommand = new CrossAsyncCommand(async () => await NavigationService.Close(this)));


    ICrossCommand _incrementCommand;

    public DictionaryBindingViewModel(ILogger<DictionaryBindingViewModel> logger)
        : base(logger)
    {
    }

    public ICrossCommand IncrementCommand =>
        _incrementCommand ?? (_incrementCommand = new CrossCommand(Increment));

    private void Increment()
    {
        if (Value < 3)
        {
            Value++;
        }
        else
        {
            Value = 0;
        }
    }
}
