namespace Nivaes.App.Cross.Sample.WinUI3
{
    using Nivaes.App.Cross.WinUI3;
    using Nivaes.IoC;

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App 
        : WinUICrossApplication
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

            // ToDo: Generación por Roslyn 
            var viewsManager = new CrossViewsManager(new[]
            {
                CrossViewsManager.New<RootViewModel, RootView>(),
                CrossViewsManager.New<NewWindowViewModel, NewWindowView>(),
            });
            Singleton<CrossViewsManager>.Add(viewsManager);

            var viewPresentationsManager = new CrossViewPresentationsManager(new[] 
            { 
                CrossViewPresentationsManager.New<RootView, CrossPageViewPresentation>(),
                CrossViewPresentationsManager.New<NewWindowView, CrossNewWindowViewPresentation>(),
            });
            Singleton<CrossViewPresentationsManager>.Add(viewPresentationsManager);

        }
    }
}
