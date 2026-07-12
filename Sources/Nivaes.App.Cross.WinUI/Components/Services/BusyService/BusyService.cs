namespace Nivaes.App.Cross.WinUI
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls.Primitives;

    public class BusyService : IBusyService
    {
        [DebuggerStepThrough]
        public ValueTask Show(Func<Task> action)
        {
            return Show(string.Empty, action);
        }

        [DebuggerStepThrough]
        public ValueTask<T> Show<T>(Func<Task<T>> action)
        {
            return Show<T>(string.Empty, action);
        }

        //[DebuggerStepThrough]
        public async ValueTask Show(string pregressText, Func<Task> action)
        {
            ArgumentNullException.ThrowIfNull(action);

            Popup busyPopup = null;
            try
            {
                //busyPopup = ShowBusy(pregressText);

                //busyPopup.IsOpen = true;

                await action.Invoke();
            }
            finally
            {
                if (busyPopup != null)
                    busyPopup.IsOpen = false;
            }
        }

        //[DebuggerStepThrough]
        public async ValueTask<T> Show<T>(string pregressText, Func<Task<T>> action)
        {
            ArgumentNullException.ThrowIfNull(action);

            Popup busyPopup = null;
            try
            {
                //busyPopup = ShowBusy(pregressText);

                //busyPopup.IsOpen = true;

                return await action.Invoke();
            }
            finally
            {
                if (busyPopup != null)
                    busyPopup.IsOpen = false;
            }
        }

        private Popup ShowBusy(string pregressText)
        {
            Popup busyPopup = null;

            var parent = (FrameworkElement)Window.Current.Content;
            var child = new BusyContentControl
            {
                Message = pregressText
            };

            busyPopup = new Popup
            {
                Child = child
            };

            parent.SizeChanged += (o, e) =>
            {
                busyPopup.HorizontalOffset = (e.NewSize.Width - child.ActualWidth) / 2;
                busyPopup.VerticalOffset = (e.NewSize.Height - child.ActualHeight) / 2;
            };
            busyPopup.Opened += (o, e) =>
            {
                busyPopup.HorizontalOffset = (parent.ActualWidth - child.ActualWidth) / 2;
                busyPopup.VerticalOffset = (parent.ActualHeight - child.ActualHeight) / 2;
            };

            return busyPopup;
        }
    }
}
