namespace Nivaes.App.Cross.Mobele.Windows.Sample.Views
{
    using MvvmCross.Platforms.Uap.Presenters.Attributes;
    using MvvmCross.Platforms.Uap.Views;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross.Sample;

    [MvxViewFor(typeof(RootViewModel))]
    [MvxPagePresentation]
    public sealed partial class RootView
        : RootViewPage
    {
        public RootView()
        {
            this.InitializeComponent();
        }
    }

    public abstract class RootViewPage
        : MvxWindowsPage<RootViewModel>
    {
    }
}
