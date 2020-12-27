namespace Nivaes.App.Cross.Sample
{
    using System.Threading.Tasks;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross.Navigation;

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
