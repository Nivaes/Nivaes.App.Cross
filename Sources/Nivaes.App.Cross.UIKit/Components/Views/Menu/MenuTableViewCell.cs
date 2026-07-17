namespace Nivaes.App.Cross.UIKitLib
{
    using System;
    using Foundation;
    using UIKit;

    public partial class MenuTableViewCell
        : UITableViewCell
    {
        public static readonly NSString Key = new NSString("MenuTableViewCell");
        public static readonly UINib Nib;

        public UILabel MenuItemTextLabel
        {
            get => LabelMenuItemName;
            set => LabelMenuItemName = value;
        }

        public UIImageView MenuImage
        {
            get => MenuItemImage;
            set => MenuItemImage = value;
        }

        static MenuTableViewCell()
        {
            Nib = UINib.FromName("MenuTableViewCell", NSBundle.MainBundle);
        }

        public static MenuTableViewCell Create()
        {
            return (MenuTableViewCell)Nib.Instantiate(null, null)[0];
        }

        protected MenuTableViewCell(IntPtr handle)
            : base(handle)
        {
        }
    }
}
