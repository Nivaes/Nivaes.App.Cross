namespace Nivaes.App.Cross.UIKit.TvOS
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Binding.BindingContext;
    using Nivaes.App.Cross;
    using ObjCRuntime;

    public class MvxCollectionViewCell
        : UICollectionViewCell, IMvxBindable
    {
        public IMvxBindingContext BindingContext { get; set; }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxCollectionViewCell(string bindingText)
        {
            this.CreateBindingContext(bindingText);
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxCollectionViewCell(NativeHandle handle)
            : base(handle)
        {
            this.CreateBindingContext();
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxCollectionViewCell(string bindingText, NativeHandle handle)
            : base(handle)
        {
            this.CreateBindingContext(bindingText);
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxCollectionViewCell(CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext();
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxCollectionViewCell(string bindingText, CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext(bindingText);
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxCollectionViewCell(IEnumerable<CrossBindingDescription> bindingDescriptions, CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext(bindingDescriptions);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                BindingContext.ClearAllBindings();
            }
            base.Dispose(disposing);
        }

        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }
    }
}
