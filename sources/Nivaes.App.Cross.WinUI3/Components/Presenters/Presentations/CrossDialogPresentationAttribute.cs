namespace Nivaes.App.Cross.WinUI3
{
    using System;
    using Microsoft.UI.Xaml.Controls;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CrossDialogPresentationAttribute : CrossPresentationAttribute
    {
        public CrossDialogPresentationAttribute(ContentDialogPlacement placement)
        {
            this.Placement = placement;
        }

        public ContentDialogPlacement Placement { get; set; }
    }
}
