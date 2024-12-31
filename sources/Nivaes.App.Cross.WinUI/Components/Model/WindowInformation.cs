namespace Nivaes.App.Cross.WinUI
{
    using Microsoft.UI.Xaml.Controls;
    using Nivaes.App.Cross.WinUI.Presenters;

    // ToDo: Cambiar nombre a la clase. (AppDataModel)
    public sealed class WindowInformation
    {
        public WrappedFrame MainFrame { get; }

        public WindowInformation(Frame mainFrame)
        {
            this.MainFrame = new WrappedFrame(mainFrame);
        }
    }
}
