namespace Playground.Core.ViewModels.Bindings
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Navigation;
    using Nivaes.App.Cross;

    public class FluentBindingViewModel : BaseViewModel
    {
        bool _bindingsEnabled = true;

        public FluentBindingViewModel(ILoggerFactory loggerFactory, IMvxNavigationService navigationService)
            : base(loggerFactory, navigationService)
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
}
