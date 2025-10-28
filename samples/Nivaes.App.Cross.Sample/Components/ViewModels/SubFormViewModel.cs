namespace Nivaes.App.Cross.Sample
{
    using System.Diagnostics;
    using System.Windows.Input;
    using System.Xml.Linq;

    public class SubFormViewModel
        : ViewModelResult<SubFormModelResult>
    {
        private readonly INavigationService mNavigationService;

        public SubFormViewModel(INavigationService navigationService)
        {
            mNavigationService = navigationService;
            //mTitle = "Root View en RootViewModel";
            //mName = "Name in view model";
            //mAge = 44;
            CommandSubSubForm = new CrossCommand(() =>
            {
                var result = mNavigationService.Navigate<SubSubFormViewModel, SubFormModelResult>().ConfigureAwait(false);

            });
            //Command.Execute("aa");
        }

        //private string? mTitle;

        //public string? Title 
        //{
        //    [DebuggerStepThrough]
        //    get => mTitle;
        //    [DebuggerStepThrough]
        //    set
        //    {
        //        if (mTitle != value)
        //        {
        //            mTitle = value;
        //            RaisePropertyChanged();
        //        }
        //    }
        //}

        //private string? mName;

        //public string? Name
        //{
        //    [DebuggerStepThrough]
        //    get => mName;
        //    [DebuggerStepThrough]
        //    set
        //    {
        //        if (mName != value)
        //        {
        //            mName = value;
        //            RaisePropertyChanged();
        //        }
        //    }
        //}

        //private int? mAge;

        //public int? Age
        //{
        //    [DebuggerStepThrough]
        //    get => mAge;
        //    [DebuggerStepThrough]
        //    set
        //    {
        //        if (mAge != value)
        //        {
        //            mAge = value;
        //            RaisePropertyChanged();
        //        }
        //    }
        //}

        private ICommand? mCommandSubSubForm;

        public ICommand? CommandSubSubForm
        {
            [DebuggerStepThrough]
            get => mCommandSubSubForm;
            [DebuggerStepThrough]
            set
            {
                if (mCommandSubSubForm != value)
                {
                    mCommandSubSubForm = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
