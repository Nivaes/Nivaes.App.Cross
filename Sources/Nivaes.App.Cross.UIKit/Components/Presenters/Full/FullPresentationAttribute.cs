namespace Nivaes.App.Cross.UIKitLib;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class FullPresentationAttribute
    : BasePresentationAttribute
{
    public FullPresentationAttribute(PanelType panelType = PanelType.Primary)
    {
        switch (panelType)
        {
            case PanelType.Primary:
                break;
            case PanelType.Secondary:
                break;
        }
    }
}
