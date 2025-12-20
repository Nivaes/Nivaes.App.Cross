namespace MvvmCross.Platforms.WinUi.Presenters
{
    using Microsoft.UI.Xaml;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    /// <summary>
    /// Defines public services for the MultiWindow support.
    /// </summary>
    public interface IMvxMultiWindowsService
    {
        /// <summary>
        /// Gets the window for the given view model.
        /// </summary>
        /// <param name="viewModel">The viewmodel instance to find the window it belongs to for</param>
        /// <returns>The window found, or the application main window if not found.</returns>
        public Window GetWindow(ICrossViewModel viewModel);
    }
}