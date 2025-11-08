namespace Nivaes.App.Cross.Droid
{
    using Android.Graphics;
    using Microsoft.Extensions.Logging;

    public abstract class CrossBaseImageViewTargetBinding(ImageView imageView)
        : CrossAndroidTargetBinding(imageView)
    {
        protected ImageView? ImageView => (ImageView?)Target;

        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWay;

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
                CrossLogHost.GetLog<CrossBaseImageViewTargetBinding>()?
                    .Log(LogLevel.Error, ex, "Failed to set bitmap on ImageView");
                throw;
            }
        }

        protected virtual void SetImageBitmap(ImageView imageView, Bitmap? bitmap) =>
            imageView.SetImageBitmap(bitmap);

        protected abstract bool GetBitmap(object? value, out Bitmap? bitmap);
    }
}