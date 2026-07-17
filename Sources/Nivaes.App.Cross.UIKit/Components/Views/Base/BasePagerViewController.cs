namespace Nivaes.App.Cross.UIKitLib
{
    /// <summary> A base view controller </summary>
    public abstract class BasePagerViewController<TViewModel>
        : BaseViewController<TViewModel>, ITabBarItemViewController
        where TViewModel : BasePagerViewModel
    {
        public virtual string? TabName => base.Title;
        public virtual string? TabIconName { get; }
        public virtual string? TabSelectedIconName { get; }

        public BasePagerViewController()
            : base()
        {
        }

        protected BasePagerViewController(IntPtr handle)
            : base(handle)
        {
        }
    }
}
