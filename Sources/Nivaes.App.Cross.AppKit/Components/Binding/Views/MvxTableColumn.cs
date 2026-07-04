using ObjCRuntime;

namespace Nivaes.App.Cross.AppKitLib;

[Register("MvxTableColumn")]
public class MvxTableColumn : NSTableColumn
{
    // Called when created from unmanaged code
    public MvxTableColumn(NativeHandle handle) : base(handle)
    {
        this.Initialize();
    }

    // Called when created directly from a XIB file
    [Export("initWithCoder:")]
    public MvxTableColumn(NSCoder coder) : base(coder)
    {
        this.Initialize();
    }

    public MvxTableColumn() : base()
    {
        this.Initialize();
    }

    // Shared initialization code
    private void Initialize()
    {
        // Method intentionally left empty.
    }

    public string BindingText
    {
        get;
        set;
    }

    public override void SetValueForKey(NSObject value, NSString key)
    {
        if (key == "bindingText")
            this.BindingText = value.ToString();
    }
}
