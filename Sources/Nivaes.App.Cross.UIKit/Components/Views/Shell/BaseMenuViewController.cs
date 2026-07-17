namespace Nivaes.App.Cross.UIKitLib
{
    /// <summary>A base menu view controller.</summary>
    public abstract class BaseMenuViewController<TViewModel>
        : MvxViewController<TViewModel>, IMenuViewController
        where TViewModel : BaseMenuViewModel
    {
        private readonly CGColor borderColor = UIColor.White.CGColor;
        private readonly UIColor textColor = UIColor.White;

        protected virtual MenuItem[] MenuItems { get; }

        protected virtual UIImageView BaseProfileImage { get; }
        protected virtual UILabel BaseBigLabel { get; }
        protected virtual UILabel BaseSmallLabel { get; }
        protected virtual UITableView BaseMenuTableView { get; }

        public override void ViewDidLoad()
        {
            base.EdgesForExtendedLayout = UIRectEdge.All;

            base.ViewDidLoad();

            BaseMenuTableView.TableFooterView = new UIView();

            //Corner radius and color
            BaseProfileImage.Layer.CornerRadius = (BaseProfileImage.Frame.Width / 2);
            BaseProfileImage.Layer.BorderWidth = 1.5f;
            BaseProfileImage.Layer.BorderColor = borderColor;
            BaseProfileImage.Layer.MasksToBounds = true;

            //Label colors
            BaseBigLabel.TextColor = textColor;
            BaseSmallLabel.TextColor = textColor;

            BaseMenuTableView.Source = new MenuTableViewSource(MenuItems);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.NavigationController.NavigationBarHidden = true;

            base.ViewWillAppear(animated);
        }
    }
}
