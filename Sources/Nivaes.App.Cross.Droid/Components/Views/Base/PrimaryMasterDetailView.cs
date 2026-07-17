using Android.Runtime;

namespace Nivaes.App.Cross.Droid
{
    [FullPresentation(PanelType.Primary)]
    [Register("com.nivaes.app.PrimaryMasterDetailView")]
    public sealed class PrimaryMasterDetailView
        : IBaseMasterDetailView<PrimaryMasterDetailViewModel>
    {
        public PrimaryMasterDetailView()
        {
        }
    }
}
