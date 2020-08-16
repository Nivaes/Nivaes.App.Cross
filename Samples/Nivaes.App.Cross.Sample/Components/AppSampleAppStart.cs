namespace Nivaes.App.Cross.Mobile.Sample
{
    using System;
    using System.Threading.Tasks;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;

    public abstract class AppSampleAppStart
        : MvxAppStart
    {
        protected AppSampleAppStart(IMvxApplication application, IMvxNavigationService navigationService)
            : base(application, navigationService)
        {
        }

        protected override Task NavigateToFirstViewModel(object? hint = null)
        {
            return Task.CompletedTask;
            //return base.NavigationService.Navigate<InitializingAppViewModel>();
        }
    }
}
