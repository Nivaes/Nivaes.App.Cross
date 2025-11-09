namespace Nivaes.App.Cross.Droid.Target
{
    using Android.Graphics;
    using AndroidX.AppCompat.Widget;
    using Microsoft.Extensions.Logging;

    public abstract class CrossAppCompatBaseImageViewTargetBinding
        : CrossAndroidTargetBinding
    {
        protected AppCompatImageView ImageView => (AppCompatImageView)Target;

        protected CrossAppCompatBaseImageViewTargetBinding(AppCompatImageView imageView)
            : base(imageView)
        {
        }

        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWay;

        protected override void SetValueImpl(object target, object value)
        {
            var imageView = (AppCompatImageView)target;

            try
            {
                if (!GetBitmap(value, out var bitmap))
                    return;
                using (bitmap)
                    SetImageBitmap(imageView, bitmap);
            }
            catch (Exception ex)
            {
                CrossBindingLog.Instance?.LogError(ex, "Failed to set value");
                throw;
            }
        }

        protected virtual void SetImageBitmap(AppCompatImageView imageView, Bitmap bitmap)
        {
            imageView.SetImageBitmap(bitmap);
        }

        protected abstract bool GetBitmap(object value, out Bitmap bitmap);
    }
}
