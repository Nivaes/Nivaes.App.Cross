namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Binding;

    public class MvxImageViewResourceNameTargetBinding(ImageView imageView)
        : MvxImageViewDrawableTargetBinding(imageView)
    {
        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWay;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(string);

        protected override void SetImage(ImageView view, int id)
        {
            view.SetImageResource(id);
        }
    }
}