namespace Nivaes.App.Cross.WinUI
{
    using Microsoft.UI.Xaml;
    using Nivaes.App.Cross;

    public class MvxVisibleTargetBinding
        : MvxDependencyPropertyTargetBinding
    {
        public MvxVisibleTargetBinding(object target)
            : base(target, "Visibility", UIElement.VisibilityProperty, typeof(Visibility))
        {
        }

        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWay;

        public override void SetValue(object value)
        {
            if (value == null)
                value = false;
            var boolValue = (bool)value;
            base.SetValue(boolValue ? Visibility.Visible : Visibility.Collapsed);
        }
    }
}
