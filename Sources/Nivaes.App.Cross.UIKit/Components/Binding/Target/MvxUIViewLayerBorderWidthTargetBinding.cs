using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross.UIKitLib
{
    public class MvxUIViewLayerBorderWidthTargetBinding(UIView target)
        : CrossConvertingTargetBinding(target)
    {
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(float);

        protected override void SetValueImpl(object target, object? value)
        {
            var view = target as UIView;
            if (view?.Layer == null || value == null) return;

            view.Layer.BorderWidth = (float)value;
        }
    }
}