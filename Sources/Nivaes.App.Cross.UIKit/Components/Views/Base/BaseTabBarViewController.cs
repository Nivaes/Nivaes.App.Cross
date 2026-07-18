namespace Nivaes.App.Cross.UIKitLib
{
    /// <summary> A base view controller </summary>
    public abstract class BaseTabBarViewController<TViewModel>
        : TabBarViewController<TViewModel>
        where TViewModel : BaseViewModel
    {
        public virtual string TabName => base.Title;
        public virtual string TabIconName { get; }
        public virtual string TabSelectedIconName { get; }

        public BaseTabBarViewController()
            : base()
        {
        }

        protected BaseTabBarViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            base.ViewControllerSelected += (o, e) => SetTitle();
        }

        protected override void SetTitleAndTabBarItem(UIViewController viewController, TabPresentationAttribute attribute)
        {
            base.SetTitleAndTabBarItem(viewController, attribute);

            SetTitle();
        }

        private void SetTitle()
        {
            if (SelectedViewController != null)
            {
                base.Title = SelectedViewController.Title;
            }
            else
            {
                base.Title = string.Empty;
            }
        }
    }
}
