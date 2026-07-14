namespace Nivaes.App.Cross.WinUI;

public class SplitViewPresentationAttribute
    : BasePresentationAttribute
{
    public SplitViewPresentationAttribute() : this(SplitPanePosition.Content)
    {
    }

    public SplitViewPresentationAttribute(SplitPanePosition position)
    {
        Position = position;
    }

    public SplitPanePosition Position { get; set; }
}

public enum SplitPanePosition
{
    Pane,
    Content
}
