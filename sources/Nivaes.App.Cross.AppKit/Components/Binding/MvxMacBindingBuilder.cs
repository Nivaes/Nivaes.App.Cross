namespace Nivaes.App.Cross.AppKit
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Platforms.Mac.Binding.Target;

    public class MvxMacBindingBuilder
        : CrossBindingBuilder
    {
        private readonly Action<ICrossTargetBindingFactoryRegistry> _fillRegistryAction;
        private readonly Action<IMvxValueConverterRegistry> _fillValueConvertersAction;
        private readonly Action<ICrossBindingNameRegistry> _fillBindingNamesAction;
        private readonly Action<ICrossValueCombinerRegistry> _fillValueCombinersAction;

        public MvxMacBindingBuilder(Action<ICrossTargetBindingFactoryRegistry> fillRegistryAction = null,
                                    Action<IMvxValueConverterRegistry> fillValueConvertersAction = null,
                                    Action<ICrossBindingNameRegistry> fillBindingNamesAction = null,
                                    Action<ICrossValueCombinerRegistry> fillValueCombinersAction = null)
        {
            _fillRegistryAction = fillRegistryAction;
            _fillValueConvertersAction = fillValueConvertersAction;
            _fillBindingNamesAction = fillBindingNamesAction;
            _fillValueCombinersAction = fillValueCombinersAction;
        }

        protected override ICrossValueCombinerRegistry CreateValueCombinerRegistry()
        {
            var registry = base.CreateValueCombinerRegistry();
            _fillValueCombinersAction?.Invoke(registry);
            return registry;
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("This method registers target bindings that may not be preserved by trimming")]
        protected override void FillTargetFactories(ICrossTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

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
            _fillRegistryAction?.Invoke(registry);
        }

        [RequiresUnreferencedCode("This method creates bindings using reflection which may not be preserved by trimming.")]
        protected virtual void RegisterPropertyInfoBindingFactory(ICrossTargetBindingFactoryRegistry registry,
                                                                  [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type bindingType,
                                                                  [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] Type targetType,
                                                                  string targetName)
        {
            registry.RegisterFactory(new CrossSimplePropertyInfoTargetBindingFactory(bindingType, targetType, targetName));
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);

            _fillValueConvertersAction?.Invoke(registry);
        }

        protected override void FillDefaultBindingNames(ICrossBindingNameRegistry registry)
        {
            base.FillDefaultBindingNames(registry);

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
            _fillBindingNamesAction?.Invoke(registry);
        }
    }
}
