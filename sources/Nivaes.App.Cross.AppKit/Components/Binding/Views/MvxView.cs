namespace Nivaes.App.Cross.AppKit
{
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using MvvmCross.Binding.BindingContext;
    using ObjCRuntime;

    public class MvxView
        : NSView
        , IMvxBindable
    {
        public IMvxBindingContext BindingContext { get; set; }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxView()
        {
            this.CreateBindingContext();
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxView(NativeHandle handle)
            : base(handle)
        {
            this.CreateBindingContext();
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxView(NSCoder coder)
            : base(coder)
        {
            this.CreateBindingContext();
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxView(RectangleF frame)
            : base(frame)
        {
            this.CreateBindingContext();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.BindingContext.ClearAllBindings();
            }
            base.Dispose(disposing);
        }

        public object DataContext
        {
            get { return this.BindingContext.DataContext; }
            set { this.BindingContext.DataContext = value; }
        }
    }
}
