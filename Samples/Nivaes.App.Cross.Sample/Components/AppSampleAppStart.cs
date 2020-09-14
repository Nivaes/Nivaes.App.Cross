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

        protected override ValueTask<bool> NavigateToFirstViewModel(object? hint = null)
        {
            return new ValueTask<bool>(false);
            //return base.NavigationService.Navigate<InitializingAppViewModel>();
        }
    }
}
