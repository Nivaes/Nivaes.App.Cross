namespace MvvmCross.Platforms.Tvos.Binding.Target
{
    using System.Diagnostics.CodeAnalysis;
    using Nivaes.App.Cross;

    public class MvxUIViewLayerBorderWidthTargetBinding
        : CrossConvertingTargetBinding
    {
        public MvxUIViewLayerBorderWidthTargetBinding(UIView target)
            : base(target)
        {
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(float);

        protected override void SetValueImpl(object target, object value)
        {
            var view = (UIView)target;

            if (view == null || value == null)
                return;

            if (view.Layer == null)
                return;

            view.Layer.BorderWidth = (float)value;
        }
    }
}
