namespace Nivaes.App.Cross.UIKit
{
    using System.Diagnostics.CodeAnalysis;

    public class CrossIosBindingBuilder
        : CrossBindingBuilder
    {
        private readonly Action<ICrossTargetBindingFactoryRegistry> _fillRegistryAction;
        private readonly Action<ICrossValueConverterRegistry> _fillValueConvertersAction;
        private readonly Action<ICrossBindingNameRegistry> _fillBindingNamesAction;
        private readonly CrossUnifiedTypesValueConverter _unifiedValueTypesConverter;
        private readonly Action<ICrossValueCombinerRegistry> _fillValueCombinersAction;

        public CrossIosBindingBuilder(Action<ICrossTargetBindingFactoryRegistry> fillRegistryAction = null,
                                    Action<ICrossValueConverterRegistry> fillValueConvertersAction = null,
                                    Action<ICrossValueCombinerRegistry> fillValueCombinersAction = null,
                                    Action<ICrossBindingNameRegistry> fillBindingNamesAction = null)
        {
            _fillRegistryAction = fillRegistryAction;
            _fillValueConvertersAction = fillValueConvertersAction;
            _fillValueCombinersAction = fillValueCombinersAction;
            _fillBindingNamesAction = fillBindingNamesAction;

            _unifiedValueTypesConverter = new CrossUnifiedTypesValueConverter();
        }

        [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        protected override void FillTargetFactories(ICrossTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterCustomBindingFactory<UIControl>(
                CrossIosPropertyBinding.UIControl_TouchDown,
                view => new CrossUIControlTargetBinding(view, CrossIosPropertyBinding.UIControl_TouchDown));

            registry.RegisterCustomBindingFactory<UIControl>(
                CrossIosPropertyBinding.UIControl_TouchDownRepeat,
                view => new CrossUIControlTargetBinding(view, CrossIosPropertyBinding.UIControl_TouchDownRepeat));

            registry.RegisterCustomBindingFactory<UIControl>(
                CrossIosPropertyBinding.UIControl_TouchDragInside,
                view => new CrossUIControlTargetBinding(view, CrossIosPropertyBinding.UIControl_TouchDragInside));

            registry.RegisterCustomBindingFactory<UIControl>(
                CrossIosPropertyBinding.UIControl_TouchUpInside,
                view => new CrossUIControlTargetBinding(view, CrossIosPropertyBinding.UIControl_TouchUpInside));

            registry.RegisterCustomBindingFactory<UIControl>(
                CrossIosPropertyBinding.UIControl_ValueChanged,
                view => new CrossUIControlTargetBinding(view, CrossIosPropertyBinding.UIControl_ValueChanged));

            registry.RegisterCustomBindingFactory<UIControl>(
                CrossIosPropertyBinding.UIControl_PrimaryActionTriggered,
                view => new CrossUIControlTargetBinding(view, CrossIosPropertyBinding.UIControl_PrimaryActionTriggered));

            registry.RegisterCustomBindingFactory<UIControl>(
                CrossIosPropertyBinding.UIControl_EditingDidBegin,
                view => new CrossUIControlTargetBinding(view, CrossIosPropertyBinding.UIControl_EditingDidBegin));

            registry.RegisterCustomBindingFactory<UIControl>(
                CrossIosPropertyBinding.UIControl_EditingChanged,
                view => new CrossUIControlTargetBinding(view, CrossIosPropertyBinding.UIControl_EditingChanged));

            registry.RegisterCustomBindingFactory<UIControl>(
                CrossIosPropertyBinding.UIControl_EditingDidEnd,
                view => new CrossUIControlTargetBinding(view, CrossIosPropertyBinding.UIControl_EditingDidEnd));

            registry.RegisterCustomBindingFactory<UIControl>(
                CrossIosPropertyBinding.UIControl_EditingDidEndOnExit,
                view => new CrossUIControlTargetBinding(view, CrossIosPropertyBinding.UIControl_EditingDidEndOnExit));

            registry.RegisterCustomBindingFactory<UIControl>(
                CrossIosPropertyBinding.UIControl_AllTouchEvents,
                view => new CrossUIControlTargetBinding(view, CrossIosPropertyBinding.UIControl_AllTouchEvents));

            registry.RegisterCustomBindingFactory<UIControl>(
                CrossIosPropertyBinding.UIControl_AllEditingEvents,
                view => new CrossUIControlTargetBinding(view, CrossIosPropertyBinding.UIControl_AllEditingEvents));

            registry.RegisterCustomBindingFactory<UIControl>(
                CrossIosPropertyBinding.UIControl_AllEvents,
                view => new CrossUIControlTargetBinding(view, CrossIosPropertyBinding.UIControl_AllEvents));

            registry.RegisterCustomBindingFactory<UIView>(
                CrossIosPropertyBinding.UIView_Visibility,
                view => new CrossUIViewVisibilityTargetBinding(view));

            registry.RegisterCustomBindingFactory<UIView>(
                CrossIosPropertyBinding.UIView_Visible,
                view => new CrossUIViewVisibleTargetBinding(view));

            registry.RegisterCustomBindingFactory<UIActivityIndicatorView>(
                CrossIosPropertyBinding.UIActivityIndicatorView_Hidden,
                activityIndicator => new CrossUIActivityIndicatorViewHiddenTargetBinding(activityIndicator));

            registry.RegisterCustomBindingFactory<UIView>(
                CrossIosPropertyBinding.UIView_Hidden,
                view => new CrossUIViewHiddenTargetBinding(view));

            registry.RegisterPropertyInfoBindingFactory(
                typeof(CrossUISliderValueTargetBinding),
                typeof(UISlider),
                CrossIosPropertyBinding.UISlider_Value);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(CrossUIStepperValueTargetBinding),
                typeof(UIStepper),
                CrossIosPropertyBinding.UIStepper_Value);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(CrossUIPageControlCurrentPageTargetBinding),
                typeof(UIPageControl),
                CrossIosPropertyBinding.UIPageControl_CurrentPage);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(CrossUISegmentedControlSelectedSegmentTargetBinding),
                typeof(UISegmentedControl),
                CrossIosPropertyBinding.UISegmentedControl_SelectedSegment);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(CrossUIDatePickerDateTargetBinding),
                typeof(UIDatePicker),
                CrossIosPropertyBinding.UIDatePicker_Date);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxUIDatePickerMinMaxTargetBinding),
                typeof(UIDatePicker),
                CrossIosPropertyBinding.UIDatePicker_MinimumDate);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxUIDatePickerMinMaxTargetBinding),
                typeof(UIDatePicker),
                CrossIosPropertyBinding.UIDatePicker_MaximumDate);

            registry.RegisterCustomBindingFactory<UIDatePicker>(
                CrossIosPropertyBinding.UIDatePicker_Time,
                view => new CrossUIDatePickerTimeTargetBinding(view, typeof(UIDatePicker).GetProperty(CrossIosPropertyBinding.UIDatePicker_Date)));

            registry.RegisterPropertyInfoBindingFactory(
                typeof(CrossUIDatePickerCountdownDurationTargetBinding),
                typeof(UIDatePicker),
                CrossIosPropertyBinding.UIDatePicker_CountDownDuration);

            registry.RegisterCustomBindingFactory<UITextField>(
                CrossIosPropertyBinding.UITextField_ShouldReturn,
                textField => new CrossUITextFieldShouldReturnTargetBinding(textField));

            registry.RegisterCustomBindingFactory<UILabel>(
                CrossIosPropertyBinding.UILabel_Text,
                view => new CrossUILabelTextTargetBinding(view));

            registry.RegisterCustomBindingFactory<UITextField>(
                CrossIosPropertyBinding.UITextField_Text,
                view => new CrossUITextFieldTextTargetBinding(view));

            registry.RegisterCustomBindingFactory<UITextView>(
                CrossIosPropertyBinding.UITextView_Text,
                view => new CrossUITextViewTextTargetBinding(view));

            registry.RegisterCustomBindingFactory<UIView>(
                CrossIosPropertyBinding.UIView_LayerBorderWidth,
                view => new CrossUIViewLayerBorderWidthTargetBinding(view));

            registry.RegisterCustomBindingFactory<UISwitch>(
                CrossIosPropertyBinding.UISwitch_On,
                uiSwitch => new CrossUISwitchOnTargetBinding(uiSwitch));

            registry.RegisterPropertyInfoBindingFactory(
                typeof(CrossUISearchBarTextTargetBinding),
                typeof(UISearchBar),
                CrossIosPropertyBinding.UISearchBar_Text);

            registry.RegisterCustomBindingFactory<UIButton>(
                CrossIosPropertyBinding.UIButton_Title,
                button => new CrossUIButtonTitleTargetBinding(button));

            registry.RegisterCustomBindingFactory<UIButton>(
                CrossIosPropertyBinding.UIButton_DisabledTitle,
                button => new CrossUIButtonTitleTargetBinding(button, UIControlState.Disabled));

            registry.RegisterCustomBindingFactory<UIButton>(
                CrossIosPropertyBinding.UIButton_HighlightedTitle,
                button => new CrossUIButtonTitleTargetBinding(button, UIControlState.Highlighted));

            registry.RegisterCustomBindingFactory<UIButton>(
                CrossIosPropertyBinding.UIButton_SelectedTitle,
                button => new CrossUIButtonTitleTargetBinding(button, UIControlState.Selected));

            registry.RegisterCustomBindingFactory<UIView>(
                CrossIosPropertyBinding.UIView_Tap,
                view => new CrossUIViewTapTargetBinding(view));

            registry.RegisterCustomBindingFactory<UIView>(
                CrossIosPropertyBinding.UIView_DoubleTap,
                view => new CrossUIViewTapTargetBinding(view, 2, 1));

            registry.RegisterCustomBindingFactory<UIView>(
                CrossIosPropertyBinding.UIView_TwoFingerTap,
                view => new CrossUIViewTapTargetBinding(view, 1, 2));

            registry.RegisterCustomBindingFactory<UITextField>(
                CrossIosPropertyBinding.UITextField_TextFocus,
                textField => new CrossUITextFieldTextFocusTargetBinding(textField));

            registry.RegisterCustomBindingFactory<UIBarButtonItem>(
                CrossIosPropertyBinding.UIBarButtonItem_Clicked,
                view => new CrossUIBarButtonItemTargetBinding(view));

            /*
            registry.RegisterCustomBindingFactory<UIView>("TwoFingerDoubleTap",
                                                          view => new MvxUIViewTapTargetBinding(view, 2, 2));
            registry.RegisterCustomBindingFactory<UIView>("ThreeFingerTap",
                                                          view => new MvxUIViewTapTargetBinding(view, 1, 3));
            registry.RegisterCustomBindingFactory<UIView>("ThreeFingerDoubleTap",
                                                          view => new MvxUIViewTapTargetBinding(view, 3, 3));
            */

            _fillRegistryAction?.Invoke(registry);
        }

        protected override void FillValueConverters(ICrossValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);

            _fillValueConvertersAction?.Invoke(registry);
        }

        protected override void FillValueCombiners(ICrossValueCombinerRegistry registry)
        {
            base.FillValueCombiners(registry);
            _fillValueCombinersAction?.Invoke(registry);
        }

        protected override void FillAutoValueConverters(ICrossAutoValueConverters autoValueConverters)
        {
            base.FillAutoValueConverters(autoValueConverters);

            //register converter for xamarin unified types
            foreach (var kvp in CrossUnifiedTypesValueConverter.UnifiedTypeConversions)
                autoValueConverters.Register(kvp.Key, kvp.Value, _unifiedValueTypesConverter);
        }

        protected override void FillDefaultBindingNames(ICrossBindingNameRegistry registry)
        {
            base.FillDefaultBindingNames(registry);

            registry.AddOrOverwrite(typeof(UIButton), CrossIosPropertyBinding.UIControl_TouchUpInside);
            registry.AddOrOverwrite(typeof(UIBarButtonItem), nameof(UIBarButtonItem.Clicked));
            registry.AddOrOverwrite(typeof(UISearchBar), CrossIosPropertyBinding.UISearchBar_Text);
            registry.AddOrOverwrite(typeof(UITextField), CrossIosPropertyBinding.UITextField_Text);
            registry.AddOrOverwrite(typeof(UITextView), CrossIosPropertyBinding.UITextView_Text);
            registry.AddOrOverwrite(typeof(UILabel), CrossIosPropertyBinding.UILabel_Text);
            registry.AddOrOverwrite(typeof(CrossCollectionViewSource), nameof(CrossCollectionViewSource.ItemsSource));
            registry.AddOrOverwrite(typeof(CrossTableViewSource), nameof(CrossTableViewSource.ItemsSource));
            registry.AddOrOverwrite(typeof(UIImageView), nameof(UIImageView.Image));
            registry.AddOrOverwrite(typeof(UIDatePicker), CrossIosPropertyBinding.UIDatePicker_Date);
            registry.AddOrOverwrite(typeof(UISlider), CrossIosPropertyBinding.UISlider_Value);
            registry.AddOrOverwrite(typeof(UISwitch), CrossIosPropertyBinding.UISwitch_On);
            registry.AddOrOverwrite(typeof(UIProgressView), nameof(UIProgressView.Progress));
            registry.AddOrOverwrite(typeof(UISegmentedControl), CrossIosPropertyBinding.UISegmentedControl_SelectedSegment);
            registry.AddOrOverwrite(typeof(UIActivityIndicatorView), CrossIosPropertyBinding.UIActivityIndicatorView_Hidden);

            _fillBindingNamesAction?.Invoke(registry);
        }
    }
}