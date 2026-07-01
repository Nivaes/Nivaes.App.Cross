#if IOS || MACCATALYST
namespace Nivaes.App.Cross.UIKitOS
{
    using UIKit;

    public interface IMvxPopoverPresentationSourceProvider
    {
        UIView? SourceView { get; set; }
        UIBarButtonItem? SourceBarButtonItem { get; set; }
        public void SetSource(UIPopoverPresentationController popoverPresentationController);
    }
}
#endif