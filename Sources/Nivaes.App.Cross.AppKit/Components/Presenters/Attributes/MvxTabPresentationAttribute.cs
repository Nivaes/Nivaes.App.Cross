namespace Nivaes.App.Cross.AppKitLib
{
    public class MvxTabPresentationAttribute
        : CrossBasePresentationAttribute
    {
        public string? WindowIdentifier { get; set; }

        public string? TabTitle { get; set; }
    }
}
