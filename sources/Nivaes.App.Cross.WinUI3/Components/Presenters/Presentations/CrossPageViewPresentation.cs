namespace Nivaes.App.Cross.WinUI3
{
    using Microsoft.UI.Xaml.Controls;
    using Nivaes.App.Cross.Presenters;

    public sealed class CrossPageViewPresentation 
        : CrossWinUIViewPresentation
    {
        public CrossPageViewPresentation(AppDataModel windowInformation)
            : base(windowInformation)
        {

        }

        //private Task<bool> ShowPage(/*IWindowsFrame rootFrame,*/ Type viewType)
        //{
        //    //_frame.Navigate(viewType);
        //}

        //private Task<bool> ClosePage(/*IWindowsFrame rootFrame,*/ Type viewType)
        //{
        //    //_frame.GoBack();
        //}
    }
}
