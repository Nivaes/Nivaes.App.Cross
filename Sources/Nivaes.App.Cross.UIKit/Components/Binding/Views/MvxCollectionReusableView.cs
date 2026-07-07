using System.Diagnostics.CodeAnalysis;
using ObjCRuntime;

namespace Nivaes.App.Cross.UIKitLib;

public class MvxCollectionReusableView
    : UICollectionReusableView, IMvxBindable
{
    public ICrossBindingContext? BindingContext { get; set; }

    [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
    public MvxCollectionReusableView()
        : this(string.Empty)
    {
    }

    [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
    public MvxCollectionReusableView(string bindingText)
    {
        this.CreateBindingContext(bindingText);
    }

    [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
    public MvxCollectionReusableView(IEnumerable<CrossBindingDescription> bindingDescriptions)
    {
        this.CreateBindingContext(bindingDescriptions);
    }

    [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
    public MvxCollectionReusableView(string bindingText, CGRect frame)
        : base(frame)
    {
        this.CreateBindingContext(bindingText);
    }

    [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
    public MvxCollectionReusableView(IEnumerable<CrossBindingDescription> bindingDescriptions, CGRect frame)
        : base(frame)
    {
        this.CreateBindingContext(bindingDescriptions);
    }

    [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
    public MvxCollectionReusableView(NativeHandle handle)
        : this(string.Empty, handle)
    {
    }

    [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
    public MvxCollectionReusableView(string bindingText, NativeHandle handle)
        : base(handle)
    {
        this.CreateBindingContext(bindingText);
    }

    [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
    public MvxCollectionReusableView(IEnumerable<CrossBindingDescription> bindingDescriptions, NativeHandle handle)
        : base(handle)
    {
        this.CreateBindingContext(bindingDescriptions);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            BindingContext?.ClearAllBindings();
        }
        base.Dispose(disposing);
    }

    [CrossSetToNullAfterBinding]
    public object? DataContext
    {
        get { return BindingContext?.DataContext; }
        set { BindingContext?.DataContext = value; }
    }
}
