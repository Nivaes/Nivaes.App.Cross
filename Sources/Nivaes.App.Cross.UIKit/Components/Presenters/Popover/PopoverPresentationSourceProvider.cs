#if IOS || MACCATALYST
using System;
using UIKit;

namespace Nivaes.App.Cross.UIKitLib
{   
    public class PopoverPresentationSourceProvider
        : IMvxPopoverPresentationSourceProvider
    {
        private readonly WeakReference<UIView?> _sourceViewWeakReference = new WeakReference<UIView?>(null);
        private readonly WeakReference<UIBarButtonItem?> _sourceBarButtonItemWeakReference = new WeakReference<UIBarButtonItem?>(null);

        public UIView? SourceView
        {
            get
            {
                if (_sourceViewWeakReference.TryGetTarget(out var view))
                    return view;

                // This is not a array Sonar. You are drunk...
                return null;
            }
            set
            {
                _sourceBarButtonItemWeakReference.SetTarget(null);
                _sourceViewWeakReference.SetTarget(value);
            }
        }

        public UIBarButtonItem? SourceBarButtonItem
        {
            get
            {
                if (_sourceBarButtonItemWeakReference.TryGetTarget(out var view))
                    return view;
                return null;
            }
            set
            {
                _sourceViewWeakReference.SetTarget(null);
                _sourceBarButtonItemWeakReference.SetTarget(value);
            }
        }

        public void SetSource(UIPopoverPresentationController popoverPresentationController)
        {
            ArgumentNullException.ThrowIfNull(popoverPresentationController);

            if (SourceView == null && SourceBarButtonItem == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(IMvxPopoverPresentationSourceProvider)} should contain a source for popover."
                );
            }

            if (SourceView != null)
            {
                popoverPresentationController.SourceView = SourceView;
                popoverPresentationController.SourceRect = SourceView.Bounds;
                SourceView = null;
            }
            else
            {
                popoverPresentationController.BarButtonItem = SourceBarButtonItem;
                SourceBarButtonItem = null;
            }
        }
    }
}
#endif