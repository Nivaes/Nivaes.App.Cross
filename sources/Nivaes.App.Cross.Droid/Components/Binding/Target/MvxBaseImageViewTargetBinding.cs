using Android.Graphics;

namespace Nivaes.App.Cross.Droid
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using MvvmCross.Logging;

    public abstract class MvxBaseImageViewTargetBinding(ImageView imageView)
    : MvxAndroidTargetBinding(imageView)
    {
        protected ImageView? ImageView => (ImageView?)Target;

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void SetValueImpl(object target, object? value)
        {
            var view = (ImageView)target;

            try
            {
                if (!GetBitmap(value, out var bitmap))
                    return;
                SetImageBitmap(view, bitmap);
            }
            catch (Exception ex)
            {
                MvxLogHost.GetLog<MvxBaseImageViewTargetBinding>()?
                    .Log(LogLevel.Error, ex, "Failed to set bitmap on ImageView");
                throw;
            }
        }

        protected virtual void SetImageBitmap(ImageView imageView, Bitmap? bitmap) =>
            imageView.SetImageBitmap(bitmap);

        protected abstract bool GetBitmap(object? value, out Bitmap? bitmap);
    }
}