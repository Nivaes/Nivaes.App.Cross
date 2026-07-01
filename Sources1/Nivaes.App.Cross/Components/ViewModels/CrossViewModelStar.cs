namespace Nivaes.App.Cross.Components.ViewModels
{
    public class CrossViewModelStar<TViewModel>
        : ICrossViewModelStar
        where TViewModel : ICrossViewModel
    {
        public CrossViewModelStar() { }

        async Task ICrossViewModelStar.NavigateToFirstViewModel(ICrossNavigationService navigationService)
        {
            await navigationService.Navigate<TViewModel>();
        }
    }
}
