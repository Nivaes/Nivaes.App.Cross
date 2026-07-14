using Nivaes.App.Cross.UIKitLib;
using ObjCRuntime;

namespace Nivaes.App.Cross.Sample.UIKitLib;

[MvxFromStoryboard("Main")]
[RootPresentation(WrapInNavigationController = true)]
public partial class TabsRootView : MvxTabBarViewController<TabsRootViewModel>
{
    private bool _isPresentedFirstTime = true;

    public TabsRootView(NativeHandle handle) : base(handle)
    {
    }

    public override void ViewWillAppear(bool animated)
    {
        base.ViewWillAppear(animated);

        if (ViewModel != null && _isPresentedFirstTime)
        {
            _isPresentedFirstTime = false;
            ViewModel.ShowInitialViewModelsCommand.ExecuteAsync(null);
        }
    }

    protected override void SetTitleAndTabBarItem(UIViewController viewController, TabPresentationAttribute attribute)
    {
        // you can override this method to set title or iconName
        if (string.IsNullOrEmpty(attribute.TabName))
            attribute.TabName = "Tab 2";
        if (string.IsNullOrEmpty(attribute.TabIconName))
            attribute.TabIconName = "ic_tabbar_menu";

        base.SetTitleAndTabBarItem(viewController, attribute);
    }

    public override bool ShowChildView(UIViewController viewController)
    {
        var type = viewController.GetType();

        return (type != typeof(ChildView)) && base.ShowChildView(viewController);
    }

    public override bool CloseChildViewModel(ICrossViewModel viewModel)
    {
        var type = viewModel.GetType();

        return (type != typeof(ChildViewModel)) && base.CloseChildViewModel(viewModel);
    }
}
