namespace Nivaes.App.Cross.Sample
{
    using System.Diagnostics;
    using System.Windows.Input;
    using System.Xml.Linq;

    public class SubFormViewModel
        : CrossViewModel
    {
        private readonly ICrossNavigationService mNavigationService;

        public SubFormViewModel(ICrossNavigationService navigationService)
        {
            mNavigationService = navigationService;
            mTitle = "Root View en RootViewModel";
            mName = "Name in view model";
            mAge = 44;
            Command = new CrossCommand(() =>
            {
                Name = "Button click.";
                Age++;
            });
            //Command.Execute("aa");
        }

        private string? mTitle;

        public string? Title 
        {
            [DebuggerStepThrough]
            get => mTitle;
            [DebuggerStepThrough]
            set
            {
                if (mTitle != value)
                {
                    mTitle = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string? mName;

        public string? Name
        {
            [DebuggerStepThrough]
            get => mName;
            [DebuggerStepThrough]
            set
            {
                if (mName != value)
                {
                    mName = value;
                    RaisePropertyChanged();
                }
            }
        }

        private int? mAge;

        public int? Age
        {
            [DebuggerStepThrough]
            get => mAge;
            [DebuggerStepThrough]
            set
            {
                if (mAge != value)
                {
                    mAge = value;
                    RaisePropertyChanged();
                }
            }
        }

        private ICommand? mCommand;

        public ICommand? Command
        {
            [DebuggerStepThrough]
            get => mCommand;
            [DebuggerStepThrough]
            set
            {
                if (mCommand != value)
                {
                    mCommand = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
