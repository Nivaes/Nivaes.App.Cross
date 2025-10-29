namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class NavigateVentArguments
    {
        public delegate void BeforeNavigateEventHandler(object sender, IMvxNavigateEventArgs e);

        public delegate void AfterNavigateEventHandler(object sender, IMvxNavigateEventArgs e);

        public delegate void BeforeCloseEventHandler(object sender, IMvxNavigateEventArgs e);

        public delegate void AfterCloseEventHandler(object sender, IMvxNavigateEventArgs e);

        public delegate void BeforeChangePresentationEventHandler(object sender, ChangePresentationEventArgs e);

        public delegate void AfterChangePresentationEventHandler(object sender, ChangePresentationEventArgs e);
    }
}
