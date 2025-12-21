namespace MvvmCross.Platforms.WinUi.Presenters.Attributes
{
    using Nivaes.App.Cross;

    public sealed class MvxRegionPresentationAttribute 
        : CrossBasePresentationAttribute
    {
        public MvxRegionPresentationAttribute(string? regionName = null)
        {
            Name = regionName;
        }

        public string? Name { get; private set; }
    }
}
