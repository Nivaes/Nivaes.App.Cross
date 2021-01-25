namespace Nivaes.App.Cross.Desktop.Windows.Sample
{
    using MvvmCross.Platforms.Wpf.Views;
    using Nivaes.App.Desktop.Sample;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
        : AppParent
    {
        //protected override void RegisterSetup()
        //{
        //    this.RegisterSetupType<MvxWpfSetup<AppDesktopSampleApplication<AppDesktopSampleAppStart>>>();
        //}
    }

    public abstract class AppParent
        : MvxApplication<Setup, AppDesktopSampleApplication<AppDesktopSampleAppStart>>
    {
    }
}
