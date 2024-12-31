namespace Nivaes.App.Cross.UIKit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class CrossUIViewController<TViewModel>
        : UIViewController, IView
        where TViewModel : class, IViewModel
    {
    }
}
