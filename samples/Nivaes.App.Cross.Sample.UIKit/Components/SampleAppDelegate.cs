namespace Nivaes.App.Cross.UIKit.Sample
{
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
                var navigationService = container.Resolve<ICrossNavigationService>();
                return new SampleApplicationStart(navigationService!);
            });

            container.AddDelegate<ICrossApplication>((container) =>
            {
                var applicationStart = container.Resolve<ICrossApplicationStart>();
                return new SampleApplication(applicationStart!);
            });

            var viewsManager = new CrossViewsManager(new[] { CrossViewsManager.New<RootViewModel, RootView>() });
            Singleton<CrossViewsManager>.Add(viewsManager);

            var viewPresentationsManager = new CrossViewPresentationsManager(new[] { CrossViewPresentationsManager.New<RootView, RootViewPresentation>() });
            Singleton<CrossViewPresentationsManager>.Add(viewPresentationsManager);
        }
    }
}
