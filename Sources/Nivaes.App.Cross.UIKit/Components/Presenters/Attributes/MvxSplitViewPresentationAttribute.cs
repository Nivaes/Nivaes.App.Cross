namespace Nivaes.App.Cross.UIKitLib
{
    public class MvxSplitViewPresentationAttribute
        : CrossBasePresentationAttribute
    {
        public MvxSplitViewPresentationAttribute(MasterDetailPosition position = MasterDetailPosition.Detail)
        {
            Position = position;

            // If this page is to be the master, the default behaviour should be that the page is not wrapped
            // in a navigation page. This is not the case for Root or Detail pages where default behaviour
            // would be to support navigation
            if (position == MasterDetailPosition.Master)
            {
                WrapInNavigationController = false;
            }
        }

        public static readonly bool DefaultWrapInNavigationController = true;
        public static readonly MasterDetailPosition DefaultPosition = MasterDetailPosition.Detail;
        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;
        public MasterDetailPosition Position { get; set; }
    }

    public enum MasterDetailPosition
    {
        Master,
        Detail
    }
}