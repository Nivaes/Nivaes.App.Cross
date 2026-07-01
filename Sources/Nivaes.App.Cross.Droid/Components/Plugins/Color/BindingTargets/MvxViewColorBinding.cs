using System.Diagnostics.CodeAnalysis;
using Android.Views;

namespace Nivaes.App.Cross.Droid
{

    public abstract class MvxViewColorBinding
        : MvxAndroidTargetBinding
    {
        protected View TextView => (View)Target!;

        protected MvxViewColorBinding(View view)
            : base(view)
        {
        }

        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWay;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(global::Android.Graphics.Color);
    }
}
