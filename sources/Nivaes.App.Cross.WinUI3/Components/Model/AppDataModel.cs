namespace Nivaes.App.Cross.WinUI3
{
    using Microsoft.UI.Xaml.Controls;

    public sealed class AppDataModel
    {
        public CrossWrappedFrame MainFrame { get; }

        public AppDataModel(Frame mainFrame)
        {
            this.MainFrame = new CrossWrappedFrame(mainFrame);
        }
    }
}
