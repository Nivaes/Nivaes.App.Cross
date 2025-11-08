namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Views;

    public abstract class CrossBaseViewVisibleBinding(object target)
        : CrossAndroidTargetBinding(target)
    {
        protected View? View => (View?)Target;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(bool);
    }
}