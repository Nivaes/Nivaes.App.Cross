namespace Nivaes.App.Cross.WinUI
{
    using System.Diagnostics;
    using Microsoft.UI.Xaml.Controls;

    public sealed partial class BusyContentControl
        : ContentControl
    {
        [DebuggerStepThrough]
        public BusyContentControl()
        {
            InitializeComponent();
        }

        public string Message
        {
            get => mMessage.Text;
            set => mMessage.Text = value;
        }
    }
}
