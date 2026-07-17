#if IOS || MACCATALYST
namespace Nivaes.App.Cross.UIKitLib
{
    using UIKit;

    public interface IPopoverPresentationSourceProvider
    {
        UIView? SourceView { get; set; }
        UIBarButtonItem? SourceBarButtonItem { get; set; }
        public void SetSource(UIPopoverPresentationController popoverPresentationController);
    }
}
#endif