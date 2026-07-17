namespace Nivaes.App.Cross.Droid
{
    using Android.Runtime;

    [FullPresentation(PanelType.Secondary)]
    [Register("com.nivaes.app.SecondaryMasterDetailView")]
    public sealed class SecondaryMasterDetailView
        : IBaseMasterDetailView<SecondaryMasterDetailViewModel>
    {
        public SecondaryMasterDetailView()
        {
        }
    }
}
