namespace Nivaes.App.Cross.WinUI3
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CrossSplitViewPresentationAttribute : CrossPresentationAttribute
    {
        public CrossSplitPanePosition Position { get; set; }

        public CrossSplitViewPresentationAttribute() : this(CrossSplitPanePosition.Content)
        {
        }

        public CrossSplitViewPresentationAttribute(CrossSplitPanePosition position)
        {
            Position = position;
        }
    }
}
