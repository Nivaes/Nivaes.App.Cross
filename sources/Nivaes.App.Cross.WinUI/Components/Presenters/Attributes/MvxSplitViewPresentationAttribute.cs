namespace Nivaes.App.Cross.WinUI;

public class MvxSplitViewPresentationAttribute 
    : CrossBasePresentationAttribute
{
    public MvxSplitViewPresentationAttribute() : this(SplitPanePosition.Content)
    {
    }

    public MvxSplitViewPresentationAttribute(SplitPanePosition position)
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
