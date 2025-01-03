namespace Nivaes.App.Cross.UIKit.Presenters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.IoC;

    public sealed class UIKitViewPresenter : ViewPresenter, IViewPresenter
    {
        //private readonly WrappedFrame mFrame;

        //public WinUIViewPresenter(WrappedFrame rootFrame)
        //{
        //    var window = (Microsoft.UI.Xaml.Application.Current as WinUIApplication)?.MainWindow;
            
        //    if (window != null)
        //    {
        //        window.AppWindow.Closing += (_, __) => CloseAllWindows();
        //    }

        //    mFrame = new WrappedFrame(rootFrame);
        //}

        public override Task<bool> Show(IViewModelRequest request)
        {
            var (viewType, viewPresentation) = GetViewPresentation(request);

            return viewPresentation!.ShowView(viewType, request);
        }

        override public Task<bool> Close(IViewModel request)
        {
            return Task.FromResult(false);
        }

        ///// <summary>
        /////     Closes all windows, except the main window, and the view models belonging to those windows.
        ///// </summary>
        //public void CloseAllWindows()
        //{
        //    //List<WindowInformation> windows;
        //    //lock (_windowInformation)
        //    //{
        //    //    windows = _windowInformation.ToList();
        //    //}

        //    //foreach (var wi in windows)
        //    //{
        //    //    try
        //    //    {
        //    //        CloseWindow(wi.Window);
        //    //    }
        //    //    catch (Exception)
        //    //    {
        //    //        // Swallow all exceptions.
        //    //    }

        //    //    wi.Window.Close();
        //    //}
        //}
    }
}
