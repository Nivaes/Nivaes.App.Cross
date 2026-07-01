namespace MvvmCross.Platforms.Mac.Presenters.Attributes
{
    using Nivaes.App.Cross;

    public class MvxSheetPresentationAttribute
        : CrossBasePresentationAttribute
    {
        public string? WindowIdentifier { get; set; }
    }
}
