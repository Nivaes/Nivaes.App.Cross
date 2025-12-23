namespace Nivaes.App.Cross.WinUI3
{
    using Microsoft.UI.Xaml;

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