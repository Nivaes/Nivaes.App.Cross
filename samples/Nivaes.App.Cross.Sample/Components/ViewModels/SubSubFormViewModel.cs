namespace Nivaes.App.Cross.Sample
{
    using System.Diagnostics;
    using System.Windows.Input;
    using System.Xml.Linq;

    public class SubSubFormViewModel
        : ViewModelResult<SubFormModelResult>
    {
        private readonly INavigationService mNavigationService;

        public SubSubFormViewModel(INavigationService navigationService)
        {
            mNavigationService = navigationService;
            //mTitle = "Root View en RootViewModel";
            //mName = "Name in view model";
            //mAge = 44;
            CommandOk = new CrossCommand(() =>
            {
                //Name = "Button click.";
                //Age++;
                base.CloseCompletionSource.SetResult(new SubFormModelResult
                {
                    StringValue = "SubSubFormViewModel",
                });
            });
        }

        public override void Prepare()
        {
            base.Prepare();
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

        private ICommand? mCommandOk;

        public ICommand? CommandOk
        {
            [DebuggerStepThrough]
            get => mCommandOk;
            [DebuggerStepThrough]
            set
            {
                if (mCommandOk != value)
                {
                    mCommandOk = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
