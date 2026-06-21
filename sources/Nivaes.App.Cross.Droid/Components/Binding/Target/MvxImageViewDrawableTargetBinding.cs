using Android.Graphics.Drawables;

namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;

    public class MvxImageViewDrawableTargetBinding(ImageView imageView)
    : MvxAndroidTargetBinding(imageView)
    {
        protected ImageView? ImageView => (ImageView?)Target;

        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWay;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(int);

        protected override void SetValueImpl(object target, object? value)
        {
            var view = (ImageView)target;

            if (value is not int resourceIdentifier)
            {
                CrossBindingLogger.Instance?.LogWarning("Value '{ResourceIdentifier}' was not a valid Drawable", value);
                view.SetImageDrawable(null);
                return;
            }

            if (resourceIdentifier == 0)
                view.SetImageDrawable(null);
            else
                SetImage(view, resourceIdentifier);
        }

        protected virtual void SetImage(ImageView view, int id)
        {
            var context = view.Context;
            Drawable? drawable = context?.Resources?.GetDrawable(id, context.Theme);
            if (drawable != null)
                view.SetImageDrawable(drawable);
        }
    }
}