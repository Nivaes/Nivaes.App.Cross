namespace Nivaes.App.Cross.UIKitLib
{
    public class MenuTableViewSource
        : UITableViewSource
    {
        private readonly IList<MenuItem> mTableItems;
        private readonly string mCellIdentifier = "MenuTableViewCell";

        public MenuTableViewSource(IList<MenuItem> menuItems)
        {
            mTableItems = menuItems;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return mTableItems.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (tableView == null) throw new ArgumentNullException(nameof(tableView));
            if (indexPath == null) throw new ArgumentNullException(nameof(indexPath));

            MenuItem item = mTableItems[indexPath.Row];

            if (!(tableView.DequeueReusableCell(mCellIdentifier) is MenuTableViewCell cell))
                cell = MenuTableViewCell.Create();

            cell.BackgroundColor = UIColor.Clear;

            cell.MenuItemTextLabel.Text = item.Label;
            cell.MenuItemTextLabel.TextColor = UIColor.DarkGray;

            cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;

            var image = UIImage.FromBundle(item.ImageName);
            if (image != null)
            {
                cell.MenuImage.Image = image;
                var templatedImage = cell.MenuImage.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                cell.MenuImage.Image = templatedImage;
                cell.MenuImage.TintColor = UIColor.Gray;
            }

            return cell;
        }

        public override async void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath == null) throw new ArgumentNullException(nameof(indexPath));

            MenuItem item = mTableItems[indexPath.Row];
            await item.Command.ExecuteAsync().ConfigureAwait(false);
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 40f;
        }
    }
}

