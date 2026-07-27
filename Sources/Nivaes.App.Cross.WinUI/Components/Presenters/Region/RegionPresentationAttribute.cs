namespace Nivaes.App.Cross.WinUI;

public sealed class RegionPresentationAttribute
    : BasePresentationAttribute
{
    public RegionPresentationAttribute(string regionName)
    {
        RegionName = regionName;
    }

    public string? RegionName { get; private set; }
}
