// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System.CodeDom.Compiler;
using Foundation;

namespace Nivaes.App.Cross.Mobile.iOS.Sample
{
    [Register ("SplitMasterView")]
    partial class SplitMasterView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnDetail { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnDetailNav { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnStack { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnDetail != null) {
                btnDetail.Dispose ();
                btnDetail = null;
            }

            if (btnDetailNav != null) {
                btnDetailNav.Dispose ();
                btnDetailNav = null;
            }

            if (btnStack != null) {
                btnStack.Dispose ();
                btnStack = null;
            }
        }
    }
}
