using System.Diagnostics.CodeAnalysis;
using ObjCRuntime;

namespace Nivaes.App.Cross.UIKitOS;

public class MvxCollectionViewListCell
    : UICollectionViewListCell, IMvxBindable
{
    public ICrossBindingContext BindingContext { get; set; }

    [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
    public MvxCollectionViewListCell()
        : this(string.Empty)
    {
    }

    [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
    public MvxCollectionViewListCell(string bindingText)
    {
        this.CreateBindingContext(bindingText);
    }

    [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
    public MvxCollectionViewListCell(IEnumerable<CrossBindingDescription> bindingDescriptions)
    {
        this.CreateBindingContext(bindingDescriptions);
    }

    [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
    public MvxCollectionViewListCell(string bindingText, CGRect frame)
        : base(frame)
    {
        this.CreateBindingContext(bindingText);
    }

    [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
    public MvxCollectionViewListCell(IEnumerable<CrossBindingDescription> bindingDescriptions, CGRect frame)
        : base(frame)
    {
        this.CreateBindingContext(bindingDescriptions);
    }

    [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
    public MvxCollectionViewListCell(NativeHandle handle)
        : this(string.Empty, handle)
    {
    }

    [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
    public MvxCollectionViewListCell(string bindingText, NativeHandle handle)
        : base(handle)
    {
        this.CreateBindingContext(bindingText);
    }

    [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
    public MvxCollectionViewListCell(IEnumerable<CrossBindingDescription> bindingDescriptions, NativeHandle handle)
        : base(handle)
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
