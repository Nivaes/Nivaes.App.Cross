namespace Nivaes.App.Cross.Sample
{
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows.Input;
    using System.Xml.Linq;

    public class RootViewModel
        : ViewModel
    {
        private readonly INavigationService mNavigationService;

        
        public RootViewModel(INavigationService navigationService)
        {
            mNavigationService = navigationService;

            ShowNewWindowCommand = new CrossCommand(() =>
            {
                mNavigationService.Navigate<NewWindowViewModel>().ConfigureAwait(false);
            });

            FormCommand = new CrossCommand(() =>
            {
                mNavigationService.Navigate<FormViewModel>().ConfigureAwait(false);
            });
        }

        private ICommand? mShowNewWindowCommand;

        public ICommand? ShowNewWindowCommand
        {
            [DebuggerStepThrough]
            get => mShowNewWindowCommand;
            [DebuggerStepThrough]
            set
            {
                if (mShowNewWindowCommand != value)
                {
                    mShowNewWindowCommand = value;
                    RaisePropertyChanged();
                }
            }
        }

        private ICommand? mFormCommand;

        public ICommand? FormCommand
        {
            [DebuggerStepThrough]
            get => mFormCommand;
            [DebuggerStepThrough]
            set
            {
                if (mFormCommand != value)
                {
                    mFormCommand = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
