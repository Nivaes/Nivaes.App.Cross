namespace Nivaes.App.Cross.UIKit
{
    using ObjCRuntime;

    public class CrossView
        : UIView
        , ICrossBindable
    {
        public ICrossBindingContext BindingContext { get; set; }

        // Constructor that will bind managed object to its unmanaged counterpart. This constructor 
        // should not have any implementation and is only used for types that can be created by the
        // interface builder (or Xamarin iOS designer). More documentation can be found:
        // - here: https://developer.xamarin.com/guides/ios/user_interface/designer/ios_designable_controls_overview/
        // - and here: https://developer.xamarin.com/guides/ios/under_the_hood/api_design/#Types_and_Interface_Builder
        public CrossView(NativeHandle handle) : base(handle) { }

        public CrossView()
        {
            this.CreateBindingContext();
        }

        public CrossView(CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext();
        }

        public CrossView(NSCoder coder)
            : base(coder)
        {
            this.CreateBindingContext();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            if (BindingContext == null)
            {
                this.CreateBindingContext();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                BindingContext.ClearAllBindings();
            }
            base.Dispose(disposing);
        }

        [CrossSetToNullAfterBinding]
        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }
    }
}
