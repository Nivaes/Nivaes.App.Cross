namespace Nivaes.App.Cross.Components.ViewModels
{
    public interface ICrossViewModelStar
    {
        internal Task NavigateToFirstViewModel(ICrossNavigationService navigationService);
    }
}
