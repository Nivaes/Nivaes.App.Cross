namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;

    public class MvxImageViewDrawableNameTargetBinding(ImageView imageView)
        : MvxImageViewDrawableTargetBinding(imageView)
    {
        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWay;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(string);

        protected override void SetValueImpl(object target, object? value)
        {
            var view = (ImageView)target;

            if (value is not string drawableName)
            {
                CrossBindingLog.Instance?.LogWarning(
                    "Value '{Value}' could not be parsed as a valid string identifier", value);
                view.SetImageDrawable(null);
                return;
            }

            var appContext = Application.Context;
            var resources = appContext.Resources;
            if (resources == null)
                return;

            var id = resources.GetIdentifier(drawableName, "drawable", appContext.PackageName);
            if (id == 0)
            {
                CrossBindingLog.Instance?.LogWarning(
                    "Value '{DrawableName}' was not a known drawable name", drawableName);
                view.SetImageDrawable(null);
                return;
            }

            base.SetValueImpl(target, id);
        }
    }
}