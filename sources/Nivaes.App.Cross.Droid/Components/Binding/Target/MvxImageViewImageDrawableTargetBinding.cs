namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Graphics.Drawables;

    public class MvxImageViewImageDrawableTargetBinding(ImageView target)
    : MvxAndroidTargetBinding(target)
    {
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(ImageView);

        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWay;

        protected override void SetValueImpl(object target, object? value)
        {
            var view = (ImageView)target;
            var drawable = value as Drawable;
            view.SetImageDrawable(drawable);
        }
    }
}