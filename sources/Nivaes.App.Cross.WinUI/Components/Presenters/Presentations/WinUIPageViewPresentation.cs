namespace Nivaes.App.Cross.WinUI.Presenters
{
    using Microsoft.UI.Xaml.Controls;
    using Nivaes.App.Cross.Presenters;

    public sealed class WinUIPageViewPresentation : WinUIViewPresentation
    {
        public WinUIPageViewPresentation(WindowInformation windowInformation)
            :base(windowInformation)
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
