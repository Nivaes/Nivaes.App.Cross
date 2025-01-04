namespace Nivaes.App.Cross.Sample
{
    public class RootViewModel
        : ViewModel
    {
        private readonly INavigationService mNavigationService;

        public RootViewModel(INavigationService navigationService)
        {
            mNavigationService = navigationService;
            mTitle = "Root View en RootViewModel";
        }

        private string mTitle;

        public string Title 
        {
            get => mTitle; 
            set
            {
                mTitle = value;
                RaisePropertyChanged();
            }
        }
    }
}
