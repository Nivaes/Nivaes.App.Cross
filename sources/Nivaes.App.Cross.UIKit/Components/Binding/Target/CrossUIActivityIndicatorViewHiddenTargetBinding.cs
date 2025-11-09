namespace Nivaes.App.Cross.UIKit
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Custom binding for UIActivityIndicator hidden.
    /// This binding will ensure the indicator animates when shown and stops when hidden
    /// </summary>
    public class CrossUIActivityIndicatorViewHiddenTargetBinding(UIActivityIndicatorView target)
        : CrossConvertingTargetBinding(target)
    {
        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWay;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(bool);

        protected UIActivityIndicatorView? View => Target as UIActivityIndicatorView;

        protected override void SetValueImpl(object target, object? value)
        {
            var view = (UIActivityIndicatorView?)target;
            if (view == null || value == null)
            {
                return;
            }

            view.Hidden = (bool)value;

            if (view.Hidden)
            {
                view.StopAnimating();
            }
            else
            {
                view.StartAnimating();
            }
        }
    }
}