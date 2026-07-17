namespace Nivaes.App.Cross.UIKitLib
{
    /// <summary> A base view controller </summary>
    public abstract class BaseSelectorDialogViewController<TViewModel>
        : BaseDialogTableViewController<TViewModel>, IUISearchResultsUpdating
        where TViewModel : ISelectorDialogViewModel
    {
        public BaseSelectorDialogViewController()
            : base()
        {

        }

        protected BaseSelectorDialogViewController(IntPtr handle)
            : base(handle)
        {
        }
    }
}
