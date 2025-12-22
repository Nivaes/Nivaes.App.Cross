namespace MvvmCross.Platforms.Tvos.Binding.Target
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;

    /// <summary>
    /// Custom binding for UIActivityIndicator hidden.
    /// This binding will ensure the indicator animates when shown and stops when hidden
    /// </summary>
    public class MvxUIActivityIndicatorViewHiddenTargetBinding 
        : CrossConvertingTargetBinding
    {
        public MvxUIActivityIndicatorViewHiddenTargetBinding(UIActivityIndicatorView target)
            : base(target)
        {
            if (target == null)
            {
                MvxBindingLog.Instance?.LogError("UIActivityIndicatorView is null in MvxUIActivityIndicatorViewHiddenTargetBinding");
            }
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(bool);

        protected UIActivityIndicatorView View => Target as UIActivityIndicatorView;

        protected override void SetValueImpl(object target, object value)
        {
            var view = (UIActivityIndicatorView)target;
            if (view == null)
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
