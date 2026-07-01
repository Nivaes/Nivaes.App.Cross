namespace Nivaes.App.Cross.UIKitOS
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using CoreGraphics;
    using Foundation;
    using Nivaes.App.Cross;
    using ObjCRuntime;
    using UIKit;

    public class MvxTableViewCell
        : UITableViewCell, IMvxBindable
    {
        public ICrossBindingContext BindingContext { get; set; }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxTableViewCell()
            : this(string.Empty)
        {
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxTableViewCell(string bindingText)
        {
            this.CreateBindingContext(bindingText);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxTableViewCell(IEnumerable<CrossBindingDescription> bindingDescriptions)
        {
            this.CreateBindingContext(bindingDescriptions);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxTableViewCell(string bindingText, CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext(bindingText);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxTableViewCell(IEnumerable<CrossBindingDescription> bindingDescriptions, CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext(bindingDescriptions);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxTableViewCell(NativeHandle handle)
            : this(string.Empty, handle)
        {
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxTableViewCell(string bindingText, NativeHandle handle)
            : base(handle)
        {
            this.CreateBindingContext(bindingText);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxTableViewCell(IEnumerable<CrossBindingDescription> bindingDescriptions, NativeHandle handle)
            : base(handle)
        {
            this.CreateBindingContext(bindingDescriptions);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxTableViewCell(string bindingText, UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(cellStyle, cellIdentifier)
        {
            Accessory = tableViewCellAccessory;
            this.CreateBindingContext(bindingText);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxTableViewCell(IEnumerable<CrossBindingDescription> bindingDescriptions,
                                UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(cellStyle, cellIdentifier)
        {
            // note that we allow the virtual Accessory property to be set here - but do not seal
            // it. Previous `sealed` code caused odd, unexplained behaviour in MonoTouch
            // - see https://github.com/MvvmCross/MvvmCross/issues/524
            Accessory = tableViewCellAccessory;
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
