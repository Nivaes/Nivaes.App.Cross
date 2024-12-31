using System;

using UIKit;
using Foundation;

namespace Nivaes.App.Cross.UIKit.Sample
{
    [Register("aaa")]
    public partial class aaa : UIViewController
    {
        public aaa() : base("aaa", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}