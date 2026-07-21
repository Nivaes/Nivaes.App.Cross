using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class FluentBindingViewModel : BaseViewModel
{
    bool _bindingsEnabled = true;

    public FluentBindingViewModel(ILogger<FluentBindingViewModel> logger)
        : base(logger)
    {
        ClearBindingsCommand = new CrossCommand(ClearBindings);
    }

    public ICrossCommand ClearBindingsCommand { get; }

    public MvxInteraction<bool> ClearBindingInteraction { get; } = new MvxInteraction<bool>();

    string _textValue;
    public string TextValue
    {
        get => _textValue;
        set => SetProperty(ref _textValue, value);
    }

    void ClearBindings()
    {
        _bindingsEnabled = !_bindingsEnabled;
        ClearBindingInteraction.Raise(_bindingsEnabled);
    }
}
