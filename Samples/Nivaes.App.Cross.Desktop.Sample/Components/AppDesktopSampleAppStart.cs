namespace Nivaes.App.Desktop.Sample
{
    using System;
    using System.Threading.Tasks;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross.Mobile.Sample;

    public sealed class AppDesktopSampleAppStart
        : AppSampleAppStart, IMvxAppStart
    {
        public AppDesktopSampleAppStart(IMvxApplication application, IMvxNavigationService navigationService)
            : base(application, navigationService)
        {
        }

        protected override Task NavigateToFirstViewModel(object? hint = null)
        {
            //return base.NavigationService.Navigate<InitializingAppViewModel>();
            throw new NotImplementedException();
        }
    }
}
