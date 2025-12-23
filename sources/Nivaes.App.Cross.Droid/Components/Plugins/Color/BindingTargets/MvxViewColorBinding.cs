using Android.Views;

namespace Nivaes.App.Cross.Droid
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross;
    using MvvmCross.Binding;
    using MvvmCross.Platforms.Android.Binding.Target;

    [Preserve(AllMembers = true)]
    public abstract class MvxViewColorBinding
        : MvxAndroidTargetBinding
    {
        protected View TextView => (View)Target;

        protected MvxViewColorBinding(View view)
            : base(view)
        {
        }

        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWay;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(global::Android.Graphics.Color);
    }
}
