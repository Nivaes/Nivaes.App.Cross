namespace MvvmCross.Platforms.WinUi.Presenters.Attributes
{
    using Microsoft.UI.Xaml.Controls;
    using Nivaes.App.Cross;

    public class MvxDialogViewPresentationAttribute : CrossBasePresentationAttribute
    {
        public MvxDialogViewPresentationAttribute()
        {
        }

        public ContentDialogPlacement Placement { get; set; }
    }
}
