namespace Nivaes.App.Desktop.Sample
{
    using System;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Navigation;
    using Nivaes.App.Cross.Sample;
    using Nivaes.App.Cross.ViewModels;

    public sealed class AppDesktopSampleAppStart
        : AppSampleAppStart, IMvxAppStart
    {
        public AppDesktopSampleAppStart(IMvxApplication application, IMvxNavigationService navigationService)
            : base(application, navigationService)
        {
        }

        protected override ValueTask<bool> NavigateToFirstViewModel(object? hint = null)
        {
            //return base.NavigationService.Navigate<InitializingAppViewModel>();
            throw new NotImplementedException();
        }
    }
}
