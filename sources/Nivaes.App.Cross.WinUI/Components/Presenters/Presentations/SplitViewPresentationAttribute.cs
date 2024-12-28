namespace Nivaes.App.Cross.WinUI.Presenters
{
    using System;
    using Nivaes.App.Cross.Presenters;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SplitViewPresentationAttribute : PresentationAttribute
    {
        public SplitPanePosition Position { get; set; }

        public SplitViewPresentationAttribute() : this(SplitPanePosition.Content)
        {
        }

        public SplitViewPresentationAttribute(SplitPanePosition position)
        {
            Position = position;
        }
    }
}
