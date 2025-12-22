namespace Playground.WinUi3.Views
{
    using MvvmCross.Platforms.WinUi.Presenters.Attributes;
    using MvvmCross.Platforms.WinUi.Views;
    using Nivaes.App.Cross;
    using Playground.Core.ViewModels.Navigation;

    [MvxViewFor(typeof(RegionViewModel))]
    [MvxRegionPresentation("PopupLocation")]
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegionView : RegionViewPage
    {
        public RegionView()
        {
            this.InitializeComponent();
        }
    }

    public abstract class RegionViewPage 
        : MvxWindowsPage<RegionViewModel>;
}
