namespace Nivaes.App.Cross.UIKit.Sample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.Sample;
    using Nivaes.App.Cross.UIKit.Presenters;
    using Nivaes.IoC;

    public class SampleAppDelegate 
        : CrossApplicationDelegate
    {
        public SampleAppDelegate()
        {
            // ToDo: Buscar una forma de hacer esto más elegante.
            var container = Singleton<CrossIoCServiceContainer>.Instance;
            container.AddDelegate<ICrossApplicationStart>((container) =>
            {
                var navigationService = container.Resolve<INavigationService>();
                return new SampleApplicationStart(navigationService!);
            });

            container.AddDelegate<ICrossApplication>((container) =>
            {
                var applicationStart = container.Resolve<ICrossApplicationStart>();
                return new SampleApplication(applicationStart!);
            });

            var viewsManager = Singleton<ViewsManager>.Instance;
            viewsManager.AddViewModelView<RootViewModel, RootView>();

            var viewPresentationsManager = Singleton<ViewPresentationsManager>.Instance;
            viewPresentationsManager.AddPresentation<RootView, RootViewPressentation>();
        }
    }
}
