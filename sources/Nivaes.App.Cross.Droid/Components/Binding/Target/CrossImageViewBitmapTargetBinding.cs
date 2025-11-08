namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Graphics;
    using Microsoft.Extensions.Logging;

    public class CrossImageViewBitmapTargetBinding(ImageView imageView)
        : CrossBaseImageViewTargetBinding(imageView)
    {
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(Bitmap);

        protected override bool GetBitmap(object? value, out Bitmap? bitmap)
        {
            if (value is not Bitmap valueBitmap)
            {
                CrossBindingLog.Instance?.LogWarning("Value was not a valid Bitmap: {Value}", value);
                bitmap = null;
                return false;
            }

            bitmap = valueBitmap;
            return true;
        }
    }
}