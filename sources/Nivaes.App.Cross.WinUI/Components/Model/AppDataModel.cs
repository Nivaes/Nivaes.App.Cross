namespace Nivaes.App.Cross.WinUI
{
    using Microsoft.UI.Xaml.Controls;
    using Nivaes.App.Cross.WinUI.Presenters;

    public sealed class AppDataModel
    {
        public WrappedFrame MainFrame { get; }

        public AppDataModel(Frame mainFrame)
        {
            this.MainFrame = new WrappedFrame(mainFrame);
        }
    }
}
