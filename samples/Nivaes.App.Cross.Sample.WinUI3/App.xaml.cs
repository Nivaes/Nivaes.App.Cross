// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Nivaes.App.Cross.Sample.WinUI
{
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.WinUI;
    using Nivaes.App.Cross.WinUI.Presenters;
    using Nivaes.IoC;

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App 
        : WinUIApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();

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

            var viewsManager = new CrossViewsManager(new[]
            {
                CrossViewsManager.New<RootViewModel, RootView>(),
                CrossViewsManager.New<NewWindowViewModel, NewWindowView>(),
            });
            Singleton<CrossViewsManager>.Add(viewsManager);

            var viewPresentationsManager = new CrossViewPresentationsManager(new[] 
            { 
                CrossViewPresentationsManager.New<RootView, PageViewPresentation>(),
                CrossViewPresentationsManager.New<NewWindowView, NewWindowViewPresentation>(),
            });
            Singleton<CrossViewPresentationsManager>.Add(viewPresentationsManager);
        }
    }
}
