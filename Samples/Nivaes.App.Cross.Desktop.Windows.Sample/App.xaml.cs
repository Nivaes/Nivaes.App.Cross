namespace Nivaes.App.Cross.Desktop.Windows.Sample
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using MvvmCross.Core;
    using MvvmCross.Platforms.Wpf.Core;
    using MvvmCross.Platforms.Wpf.Views;
    using Nivaes.App.Desktop.Sample;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
        : MvxApplication
    {
        protected override void RegisterSetup()
        {
            this.RegisterSetupType<MvxWpfSetup<AppDesktopSampleApplication<AppDesktopSampleAppStart>>>();
        }
    }
}
