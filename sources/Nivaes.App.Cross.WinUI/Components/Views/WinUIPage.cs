namespace Nivaes.App.Cross.WinUI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.UI.Xaml.Controls;

    public class WinUIPage<TViewModel>
        : Page
        where TViewModel : class, IViewModel
    {
    }
}
