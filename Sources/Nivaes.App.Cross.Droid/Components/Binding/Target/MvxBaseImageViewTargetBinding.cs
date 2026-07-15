using Android.Graphics;

namespace Nivaes.App.Cross.Droid
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public abstract class MvxBaseImageViewTargetBinding(ImageView imageView)
    : MvxAndroidTargetBinding(imageView)
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
                var logger = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<ILogger<MvxBaseImageViewTargetBinding>>();
                logger!.Log(LogLevel.Error, ex, "Failed to set bitmap on ImageView");
                throw;
            }
        }

        protected virtual void SetImageBitmap(ImageView imageView, Bitmap? bitmap) =>
            imageView.SetImageBitmap(bitmap);

        protected abstract bool GetBitmap(object? value, out Bitmap? bitmap);
    }
}