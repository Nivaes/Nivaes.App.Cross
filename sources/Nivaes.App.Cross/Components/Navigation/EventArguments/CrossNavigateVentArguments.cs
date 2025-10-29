namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CrossNavigateVentArguments
    {
        public delegate void BeforeNavigateEventHandler(object sender, ICrossNavigateEventArgs e);

        public delegate void AfterNavigateEventHandler(object sender, ICrossNavigateEventArgs e);

        public delegate void BeforeCloseEventHandler(object sender, ICrossNavigateEventArgs e);

        public delegate void AfterCloseEventHandler(object sender, ICrossNavigateEventArgs e);

        public delegate void BeforeChangePresentationEventHandler(object sender, ChangePresentationEventArgs e);

        public delegate void AfterChangePresentationEventHandler(object sender, ChangePresentationEventArgs e);
    }
}
