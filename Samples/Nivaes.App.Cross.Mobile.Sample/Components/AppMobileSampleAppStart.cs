namespace Nivaes.App.Cross.Mobile.Sample
{
    using System.Threading.Tasks;
    using Nivaes.App.Cross.ViewModels;
    using Nivaes.App.Cross.Navigation;
    using Nivaes.App.Cross.Sample;

    public sealed class AppMobileSampleAppStart
        : AppSampleAppStart, IMvxAppStart
    {
        public AppMobileSampleAppStart(IMvxApplication application, IMvxNavigationService navigationService)
            : base(application, navigationService)
        {
        }

        protected override ValueTask<bool> NavigateToFirstViewModel(object? hint = null)
        {
            return base.NavigationService.Navigate<RootViewModel>();
        }
    }
}
