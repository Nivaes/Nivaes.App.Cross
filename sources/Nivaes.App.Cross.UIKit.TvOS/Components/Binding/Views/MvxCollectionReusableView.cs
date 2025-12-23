namespace Nivaes.App.Cross.UIKit.TvOS
{
    using MvvmCross.Binding.BindingContext;
    using Nivaes.App.Cross;
    using ObjCRuntime;

    public class MvxCollectionReusableView
        : UICollectionReusableView
          , IMvxBindable
    {
        public IMvxBindingContext BindingContext { get; set; }

        public MvxCollectionReusableView()
        {
            this.CreateBindingContext();
        }

        public MvxCollectionReusableView(NativeHandle handle)
            : base(handle)
        {
            this.CreateBindingContext();
        }

        public MvxCollectionReusableView(CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext();
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
