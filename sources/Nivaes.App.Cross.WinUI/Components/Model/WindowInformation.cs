using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace Nivaes.App.Cross.WinUI
{
    public sealed class WindowInformation
    {
        public Frame MainFrame { get; }

        public WindowInformation(Frame mainFrame)
        {
            this.MainFrame = mainFrame;
        }
    }
}
