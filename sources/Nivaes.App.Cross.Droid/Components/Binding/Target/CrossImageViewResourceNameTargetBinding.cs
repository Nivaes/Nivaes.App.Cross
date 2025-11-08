namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;

    public class CrossImageViewResourceNameTargetBinding(ImageView imageView)
        : CrossImageViewDrawableTargetBinding(imageView)
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