namespace Nivaes.App.Cross.AppKit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class CrossUIViewController<TViewModel>
        : NSViewController, IView
        where TViewModel : class, IViewModel
    {
    }
}
