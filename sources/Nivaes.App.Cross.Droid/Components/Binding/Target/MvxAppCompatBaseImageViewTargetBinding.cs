using Android.Graphics;
using AndroidX.AppCompat.Widget;

namespace Nivaes.App.Cross.Droid
{
    using System;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using MvvmCross.Exceptions;

    public abstract class MvxAppCompatBaseImageViewTargetBinding
        : MvxAndroidTargetBinding
    {
        protected AppCompatImageView ImageView => (AppCompatImageView)Target;

        protected MvxAppCompatBaseImageViewTargetBinding(AppCompatImageView imageView)
            : base(imageView)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

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
                MvxBindingLog.Instance?.LogError(ex, "Failed to set value");
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
