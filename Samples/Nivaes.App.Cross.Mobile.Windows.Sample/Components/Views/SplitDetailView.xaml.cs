// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Nivaes.App.Cross.Mobele.Windows.Sample.Views
{
    using MvvmCross.Platforms.Uap.Presenters.Attributes;
    using MvvmCross.Platforms.Uap.Views;
    using Nivaes.App.Mobile.Sample;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [MvxSplitViewPresentation(Position = SplitPanePosition.Content)]
    public sealed partial class SplitDetailView : SplitDetailViewPage
    {
        public SplitDetailView()
        {
            this.InitializeComponent();
        }
    }

    public abstract class SplitDetailViewPage : MvxWindowsPage<SplitDetailViewModel>
    {
    }
}
