namespace Nivaes.App.Cross
{
    public interface ICrossViewModelStar
    {
        internal Task NavigateToFirstViewModel(ICrossNavigationService navigationService);
    }
}
