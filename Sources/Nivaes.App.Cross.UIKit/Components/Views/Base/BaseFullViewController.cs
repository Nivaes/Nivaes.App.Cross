namespace Nivaes.App.Cross.UIKitLib
{
    /// <summary> A base full view controller </summary>
    public abstract class BaseFullViewController<TViewModel>
        : BaseViewController<TViewModel>
        where TViewModel : FullViewModel
    {
        public BaseFullViewController()
            : base()
        {
        }

        protected BaseFullViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.EdgesForExtendedLayout = UIKit.UIRectEdge.None;

            base.ViewDidLoad();
        }
    }
}
