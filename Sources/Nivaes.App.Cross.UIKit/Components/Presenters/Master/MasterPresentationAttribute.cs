namespace Nivaes.App.Cross.UIKitLib;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class MasterPresentationAttribute
    : BasePresentationAttribute
{
    internal PanelType PanelType { get; set; }

    public MasterPresentationAttribute(PanelType panelType = PanelType.Primary)
    {
        PanelType = panelType;
    }
}
