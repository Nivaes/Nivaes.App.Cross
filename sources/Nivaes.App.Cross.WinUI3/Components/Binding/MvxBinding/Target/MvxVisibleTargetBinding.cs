namespace MvvmCross.Platforms.WinUi.Binding.MvxBinding.Target
{
    using System;
    using Microsoft.UI.Xaml;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;

    public class MvxVisibleTargetBinding
        : MvxDependencyPropertyTargetBinding
    {
        public MvxVisibleTargetBinding(object target)
            : base(target, "Visibility", UIElement.VisibilityProperty, typeof(Visibility))
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override void SetValue(object value)
        {
            if (value == null)
                value = false;
            var boolValue = (bool)value;
            base.SetValue(boolValue ? Visibility.Visible : Visibility.Collapsed);
        }
    }
}
