namespace MvvmCross.Platforms.WinUi.Views
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public interface IMvxWindowsViewModelLoader
    {
        ICrossViewModel Load(string requestText, IMvxBundle savedState);
    }
}
