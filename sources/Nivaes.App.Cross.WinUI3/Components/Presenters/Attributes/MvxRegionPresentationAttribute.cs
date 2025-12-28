namespace Nivaes.App.Cross.WinUI3;

public sealed class MvxRegionPresentationAttribute 
    : CrossBasePresentationAttribute
{
    public MvxRegionPresentationAttribute(string? regionName = null)
    {
        Name = regionName;
    }

    public string? Name { get; private set; }
}
