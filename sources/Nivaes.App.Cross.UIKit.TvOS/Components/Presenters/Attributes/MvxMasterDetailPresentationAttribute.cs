namespace MvvmCross.Platforms.Tvos.Presenters.Attributes
{
    using Nivaes.App.Cross;

    public class MvxMasterDetailPresentationAttribute 
        : CrossBasePresentationAttribute
    {
        public static bool DefaultWrapInNavigationController = true;
        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;
        public MasterDetailPosition Position { get; set; }

        public MvxMasterDetailPresentationAttribute(MasterDetailPosition position = MasterDetailPosition.Detail)
        {
            Position = position;
        }
    }

    public enum MasterDetailPosition
    {
        Root,
        Master,
        Detail
    }
}
