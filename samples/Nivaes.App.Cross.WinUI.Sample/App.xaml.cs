using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Nivaes.App.Cross.Sample;
using Nivaes.IoC;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Nivaes.App.Cross.WinUI.Sample
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : WinUIApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();

            // ToDo: Buscar una forma de hacer esto más elegante.
            Singleton<CrossIoCServiceContainer>.Instance.AddDelegate<ICrossApplicationStart>((container) =>
            {
                var navigationService = container.Resolve<INavigationService>();
                return new SampleApplicationStart(navigationService!);
            });

           Singleton<CrossIoCServiceContainer>.Instance.AddDelegate<ICrossApplication>((container) =>
                {
                    var applicationStart = container.Resolve<ICrossApplicationStart>();
                    return new SampleApplication(applicationStart!);
                });
        }
    }
}
