namespace Nivaes.App.Cross.Mobile.Sample
{
    using System.Threading.Tasks;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
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
