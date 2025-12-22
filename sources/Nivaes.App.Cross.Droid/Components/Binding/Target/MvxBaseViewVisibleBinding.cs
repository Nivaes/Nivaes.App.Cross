using Android.Views;

namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;

    public abstract class MvxBaseViewVisibleBinding(object target)
    : MvxAndroidTargetBinding(target)
    {
        protected View? View => (View?)Target;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(bool);
    }
}