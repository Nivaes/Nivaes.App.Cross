using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross.AppKitLib
{
    public static class CrossBindingExtension
    {
        extension(IServiceProvider service)
        {
            public IServiceProvider TargetBindingFactoryRegistry()
            {
                // Registrar con roslyn.

                var registry = service.GetRequiredService<ICrossTargetBindingFactoryRegistry>();

                registry.RegisterCustomBindingFactory<NSView>(
                     MvxMacPropertyBinding.NSView_Visibility,
                     view => new MvxNSViewVisibilityTargetBinding(view));

                registry.RegisterCustomBindingFactory<NSView>(
                    MvxMacPropertyBinding.NSView_Visible,
                    view => new MvxNSViewVisibleTargetBinding(view));

                registry.RegisterPropertyInfoBindingFactory(
                    typeof(MvxNSSliderValueTargetBinding),
                    typeof(NSSlider),
                    MvxMacPropertyBinding.NSSlider_IntValue);

                registry.RegisterPropertyInfoBindingFactory(
                    typeof(MvxNSSegmentedControlSelectedSegmentTargetBinding),
                    typeof(NSSegmentedControl),
                    MvxMacPropertyBinding.NSSegmentedControl_SelectedSegment);

                registry.RegisterPropertyInfoBindingFactory(
                    typeof(MvxNSPopUpButtonSelectedTagTargetBinding),
                    typeof(NSPopUpButton),
                    MvxMacPropertyBinding.NSPopUpButton_SelectedTag);

                registry.RegisterCustomBindingFactory<NSDatePicker>(
                    MvxMacPropertyBinding.NSDatePicker_Time,
                    view => new MvxNSDatePickerTimeTargetBinding(view));

                registry.RegisterCustomBindingFactory<NSDatePicker>(
                    MvxMacPropertyBinding.NSDatePicker_Date,
                    view => new MvxNSDatePickerDateTargetBinding(view));

                registry.RegisterPropertyInfoBindingFactory(
                    typeof(MvxNSTextFieldTextTargetBinding),
                    typeof(NSTextField),
                    MvxMacPropertyBinding.NSTextField_StringValue);

                registry.RegisterCustomBindingFactory<NSTextView>(
                    MvxMacPropertyBinding.NSTextView_StringValue,
                    view => new MvxNSTextViewTextTargetBinding(view)
                    );

                registry.RegisterPropertyInfoBindingFactory(
                    typeof(MvxNSSwitchOnTargetBinding),
                    typeof(NSButton),
                    MvxMacPropertyBinding.NSButton_State);

                registry.RegisterPropertyInfoBindingFactory(
                    typeof(MvxNSMenuItemOnTargetBinding),
                    typeof(NSMenuItem),
                    MvxMacPropertyBinding.NSMenuItem_State);

                registry.RegisterPropertyInfoBindingFactory(
                    typeof(MvxNSSearchFieldTextTargetBinding),
                    typeof(NSSearchField),
                    MvxMacPropertyBinding.NSSearchField_Text);

                registry.RegisterCustomBindingFactory<NSButton>(
                    MvxMacPropertyBinding.NSButton_Title,
                    button => new MvxNSButtonTitleTargetBinding(button));

                registry.RegisterPropertyInfoBindingFactory(
                    typeof(MvxNSTabViewControllerSelectedTabViewItemIndexTargetBinding),
                    typeof(NSTabViewController),
                    MvxMacPropertyBinding.NSTabViewController_SelectedTabViewItemIndex);

                /* Todo: Address this for trackpad
                registry.RegisterCustomBindingFactory<NSView>("Tap", view => new MvxNSViewTapTargetBinding(view));
                registry.RegisterCustomBindingFactory<NSView>("DoubleTap", view => new MvxNSViewTapTargetBinding(view, 2, 1));
                registry.RegisterCustomBindingFactory<NSView>("TwoFingerTap", view => new MvxNSViewTapTargetBinding(view, 1, 2));
                */

                return service;
            }

            public IServiceProvider BindingNameRegister()
            {
                var registry = service.GetRequiredService<ICrossBindingNameRegistry>();

                registry.AddOrOverwrite(typeof(NSButton), nameof(NSButton.Activated));
                registry.AddOrOverwrite(typeof(NSButtonCell), nameof(NSButtonCell.Activated));
                registry.AddOrOverwrite(typeof(NSMenuItem), nameof(NSMenuItem.Activated));
                registry.AddOrOverwrite(typeof(NSSearchField), MvxMacPropertyBinding.NSSearchField_Text);
                registry.AddOrOverwrite(typeof(NSTextField), MvxMacPropertyBinding.NSTextField_StringValue);
                registry.AddOrOverwrite(typeof(NSTextView), MvxMacPropertyBinding.NSTextView_StringValue);
                registry.AddOrOverwrite(typeof(NSImageView), nameof(NSImageView.Image));
                registry.AddOrOverwrite(typeof(NSDatePicker), MvxMacPropertyBinding.NSDatePicker_Date);
                registry.AddOrOverwrite(typeof(NSSlider), MvxMacPropertyBinding.NSSlider_IntValue);
                registry.AddOrOverwrite(typeof(NSSegmentedControl), MvxMacPropertyBinding.NSSegmentedControl_SelectedSegment);
                registry.AddOrOverwrite(typeof(NSPopUpButton), MvxMacPropertyBinding.NSPopUpButton_SelectedTag);
                registry.AddOrOverwrite(typeof(NSTabViewController), MvxMacPropertyBinding.NSTabViewController_SelectedTabViewItemIndex);

                //registry.AddOrOverwrite(typeof(MvxCollectionViewSource), "ItemsSource");
                //registry.AddOrOverwrite(typeof(MvxTableViewSource), "ItemsSource");

                return service;
            }
        }
    }
}
