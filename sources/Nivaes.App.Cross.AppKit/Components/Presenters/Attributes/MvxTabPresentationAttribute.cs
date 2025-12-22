namespace MvvmCross.Platforms.Mac.Presenters.Attributes
{
    using Nivaes.App.Cross;

    public class MvxTabPresentationAttribute 
        : CrossBasePresentationAttribute
    {
        public string? WindowIdentifier { get; set; }

        public string? TabTitle { get; set; }
    }
}
