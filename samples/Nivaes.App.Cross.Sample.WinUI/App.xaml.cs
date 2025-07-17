using Nivaes.App.Cross.Presenters;
using Nivaes.App.Cross.WinUI;
using Nivaes.App.Cross.WinUI.Presenters;
using Nivaes.IoC;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Nivaes.App.Cross.Sample.WinUI
{
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
                    var navigationService = container.Resolve<INavigationService>();
                    return new SampleApplicationStart(navigationService!);
                });

            container.AddDelegate<ICrossApplication>((container) =>
                {
                    var applicationStart = container.Resolve<ICrossApplicationStart>();
                    return new SampleApplication(applicationStart!);
                });

            var viewsManager = new ViewsManager(new[]
            {
                ViewsManager.New<RootViewModel, RootView>(),
                ViewsManager.New<NewWindowViewModel, NewWindowView>(),
            });
            Singleton<ViewsManager>.Add(viewsManager);

            var viewPresentationsManager = new ViewPresentationsManager(new[] 
            { 
                ViewPresentationsManager.New<RootView, PageViewPresentation>(),
                ViewPresentationsManager.New<NewWindowView, NewWindowViewPresentation>(),
            });
            Singleton<ViewPresentationsManager>.Add(viewPresentationsManager);
        }
    }
}
