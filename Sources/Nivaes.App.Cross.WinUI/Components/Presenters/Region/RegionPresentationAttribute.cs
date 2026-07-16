namespace Nivaes.App.Cross.WinUI;

public sealed class RegionPresentationAttribute
    : BasePresentationAttribute
{
    public RegionPresentationAttribute(string? regionName = null)
    {
        Name = regionName;
    }

    public string? Name { get; private set; }
}
