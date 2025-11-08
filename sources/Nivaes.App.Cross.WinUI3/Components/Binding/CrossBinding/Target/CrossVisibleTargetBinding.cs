namespace Nivaes.App.Cross.WinUI3
{
    using Microsoft.UI.Xaml;

    public class CrossVisibleTargetBinding : CrossDependencyPropertyTargetBinding
    {
        public CrossVisibleTargetBinding(object target)
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
