namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Graphics.Drawables;
    using Microsoft.Extensions.Logging;

    public class CrossImageViewDrawableTargetBinding(ImageView imageView)
        : CrossAndroidTargetBinding(imageView)
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
                CrossBindingLog.Instance?.LogWarning("Value '{ResourceIdentifier}' was not a valid Drawable", value);
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