namespace MvvmCross.Platforms.WinUi.Views
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxWindowsViewModelRequestTranslator
    {
        string GetRequestTextFor(MvxViewModelRequest request);

        // Important: if calling GetRequestTextWithKeyFor then you must later call RemoveSubViewModelWithKey on the returned key
        string GetRequestTextWithKeyFor(ICrossViewModel existingViewModelToUse);

        void RemoveSubViewModelWithKey(int key);

        int RequestTextGetKey(string requestText);
    }
}
