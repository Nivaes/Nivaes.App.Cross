using Android.Graphics;

namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;

    public class MvxImageViewBitmapTargetBinding(ImageView imageView)
    : MvxBaseImageViewTargetBinding(imageView)
    {
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(Bitmap);

        protected override bool GetBitmap(object? value, out Bitmap? bitmap)
        {
            if (value is not Bitmap valueBitmap)
            {
                CrossBindingLogger.Instance?.LogWarning("Value was not a valid Bitmap: {Value}", value);
                bitmap = null;
                return false;
            }

            bitmap = valueBitmap;
            return true;
        }
    }
}