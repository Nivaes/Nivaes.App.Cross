// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using UIKit;

namespace Nivaes.App.Cross.UIKit
{
    public static class CrossIosPropertyBindingExtensions
    {
        public static string BindTouchUpInside(this UIControl uiControl)
            => CrossIosPropertyBinding.UIControl_TouchUpInside;

        public static string BindValueChanged(this UIControl uiControl)
            => CrossIosPropertyBinding.UIControl_ValueChanged;

        public static string BindTouchDown(this UIControl uiControl)
            => CrossIosPropertyBinding.UIControl_TouchDown;

        public static string BindTouchDownRepeat(this UIControl uiControl)
            => CrossIosPropertyBinding.UIControl_TouchDownRepeat;

        public static string BindTouchDragInside(this UIControl uiControl)
            => CrossIosPropertyBinding.UIControl_TouchDragInside;

        public static string BindPrimaryActionTriggered(this UIControl uiControl)
            => CrossIosPropertyBinding.UIControl_PrimaryActionTriggered;

        public static string BindEditingDidBegin(this UIControl uiControl)
            => CrossIosPropertyBinding.UIControl_EditingDidBegin;

        public static string BindEditingChanged(this UIControl uiControl)
            => CrossIosPropertyBinding.UIControl_EditingChanged;

        public static string BindEditingDidEnd(this UIControl uiControl)
            => CrossIosPropertyBinding.UIControl_EditingDidEnd;

        public static string BindEditingDidEndOnExit(this UIControl uiControl)
            => CrossIosPropertyBinding.UIControl_EditingDidEndOnExit;

        public static string BindAllTouchEvents(this UIControl uiControl)
            => CrossIosPropertyBinding.UIControl_AllTouchEvents;

        public static string BindAllEditingEvents(this UIControl uiControl)
            => CrossIosPropertyBinding.UIControl_AllEditingEvents;

        public static string BindAllEvents(this UIControl uiControl)
            => CrossIosPropertyBinding.UIControl_AllEvents;

        public static string BindVisibility(this UIView uiView)
            => CrossIosPropertyBinding.UIView_Visibility;

        public static string BindVisible(this UIView uiView)
            => CrossIosPropertyBinding.UIView_Visible;

        public static string BindHidden(this UIActivityIndicatorView uiActivityIndicatorView)
             => CrossIosPropertyBinding.UIActivityIndicatorView_Hidden;

        public static string BindHidden(this UIView uiView)
            => CrossIosPropertyBinding.UIView_Hidden;

        public static string BindValue(this UISlider uiSlider)
            => CrossIosPropertyBinding.UISlider_Value;

        public static string BindValue(this UIStepper uiStepper)
            => CrossIosPropertyBinding.UIStepper_Value;

        public static string BindSelectedSegment(this UISegmentedControl uiSegmentedControl)
            => CrossIosPropertyBinding.UISegmentedControl_SelectedSegment;

        public static string BindDate(this UIDatePicker uiDatePicker)
            => CrossIosPropertyBinding.UIDatePicker_Date;

        public static string BindCountDownDuration(this UIDatePicker uiDatePicker)
            => CrossIosPropertyBinding.UIDatePicker_CountDownDuration;

        public static string BindShouldReturn(this UITextField uiTextField)
            => CrossIosPropertyBinding.UITextField_ShouldReturn;

        public static string BindTime(this UIDatePicker uiDatePicker)
            => CrossIosPropertyBinding.UIDatePicker_Time;

        public static string BindText(this UILabel uiLabel)
            => CrossIosPropertyBinding.UILabel_Text;

        public static string BindText(this UITextField uiTextField)
            => CrossIosPropertyBinding.UITextField_Text;

        public static string BindText(this UITextView uiTextView)
            => CrossIosPropertyBinding.UITextView_Text;

        public static string BindLayerBorderWidth(this UIView uiView)
            => CrossIosPropertyBinding.UIView_LayerBorderWidth;

        public static string BindOn(this UISwitch uiSwitch)
            => CrossIosPropertyBinding.UISwitch_On;

        public static string BindText(this UISearchBar uiSearchBar)
            => CrossIosPropertyBinding.UISearchBar_Text;

        public static string BindTitle(this UIButton uiButton)
            => CrossIosPropertyBinding.UIButton_Title;

        public static string BindDisabledTitle(this UIButton uiButton)
            => CrossIosPropertyBinding.UIButton_DisabledTitle;

        public static string BindHighlightedTitle(this UIButton uiButton)
            => CrossIosPropertyBinding.UIButton_HighlightedTitle;

        public static string BindSelectedTitle(this UIButton uiButton)
            => CrossIosPropertyBinding.UIButton_SelectedTitle;

        public static string BindTap(this UIView uiView)
            => CrossIosPropertyBinding.UIView_Tap;

        public static string BindDoubleTap(this UIView uiView)
            => CrossIosPropertyBinding.UIView_DoubleTap;

        public static string BindTwoFingerTap(this UIView uiView)
            => CrossIosPropertyBinding.UIView_TwoFingerTap;

        public static string BindTextFocus(this UIView uiView)
            => CrossIosPropertyBinding.UITextField_TextFocus;

        public static string BindClicked(this UIBarButtonItem uiBarButtonItem)
            => CrossIosPropertyBinding.UIBarButtonItem_Clicked;
    }
}
