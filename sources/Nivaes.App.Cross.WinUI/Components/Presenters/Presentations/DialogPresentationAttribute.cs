namespace Nivaes.App.Cross.WinUI.Presenters
{
    using System;
    using Microsoft.UI.Xaml.Controls;
    using Nivaes.App.Cross.Presenters;

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DialogPresentationAttribute : CrossPresentationAttribute
    {
        public DialogPresentationAttribute(ContentDialogPlacement placement)
        {
            this.Placement = placement;
        }

        public ContentDialogPlacement Placement { get; set; }
    }
}
