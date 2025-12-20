namespace MvvmCross.Platforms.WinUi.Views
{
    using Nivaes.App.Cross;

    public interface IMvxStoreViewsContainer
        : ICrossViewsContainer
            , IMvxWindowsViewModelLoader
            , IMvxWindowsViewModelRequestTranslator
    {
    }
}
