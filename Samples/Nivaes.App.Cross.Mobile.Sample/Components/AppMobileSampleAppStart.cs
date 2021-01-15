namespace Nivaes.App.Cross.Mobile.Sample
{
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Navigation;
    using Nivaes.App.Cross.Sample;
    using Nivaes.App.Cross.ViewModels;

    public sealed class AppMobileSampleAppStart
        : AppSampleAppStart, IMvxAppStart
    {
        public AppMobileSampleAppStart(ICrossApplication application, IMvxNavigationService navigationService)
            : base(application, navigationService)
        {
        }

        protected override ValueTask<bool> NavigateToFirstViewModel(object? hint = null)
        {
            return base.NavigationService.Navigate<RootViewModel>();
        }
    }
}
