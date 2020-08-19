namespace Nivaes.App.Mobile.Sample
{
    using System;
    using System.Threading.Tasks;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross.Mobile.Sample;

    public sealed class AppMobileSampleAppStart
        : AppSampleAppStart, IMvxAppStart
    {
        public AppMobileSampleAppStart(IMvxApplication application, IMvxNavigationService navigationService)
            : base(application, navigationService)
        {
        }

        protected override Task NavigateToFirstViewModel(object? hint = null)
        {
            return base.NavigationService.Navigate<RootViewModel>();
        }
    }
}
