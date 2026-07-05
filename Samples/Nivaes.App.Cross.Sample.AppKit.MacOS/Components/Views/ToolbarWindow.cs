using Nivaes.App.Cross.AppKitLib;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.AppKitOS.MacOS;

[MvxFromStoryboard("Main")]
public partial class ToolbarWindow : MvxWindowController
{
    private static int _count;

    public ToolbarWindow(NativeHandle handle) : base(handle)
    {
        _count++;
    }

    public NSTextField TextTitle => textTitle;

    public NSMenuItem MenuItem1 => menuItem1;

    public NSMenuItem MenuItem2 => menuItem2;

    public NSMenuItem MenuItem3 => menuItem3;

    public NSMenuItem MenuItemSetting => menuItemSetting;

    public NSPopUpButton PopupModes => popupModes;
}
