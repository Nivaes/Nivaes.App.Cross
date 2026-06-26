namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Content.Res;
    using Android.Graphics;
    using Android.Graphics.Drawables;
    using Microsoft.Extensions.Logging;

    public class MvxImageViewImageTargetBinding
    : MvxBaseImageViewTargetBinding
    {
        public MvxImageViewImageTargetBinding(ImageView imageView) : base(imageView) { }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(string);

        protected override bool GetBitmap(object? value, out Bitmap? bitmap)
        {
            using var assetStream = GetStream(value);
            if (assetStream == null)
            {
                bitmap = null;
                return false;
            }

            var options = new BitmapFactory.Options { InPurgeable = true };
            bitmap = BitmapFactory.DecodeStream(assetStream, null, options);
            return true;
        }

        protected override void SetImageBitmap(ImageView imageView, Bitmap? bitmap)
        {
            var drawable = new BitmapDrawable(Resources.System, bitmap);
            imageView.SetImageDrawable(drawable);
        }

        private static Stream? GetStream(object? value)
        {
            if (value == null)
            {
                CrossBindingLogger.Instance?.LogWarning("Null value passed to ImageView binding");
                return null;
            }

            var stringValue = value as string;
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                CrossBindingLogger.Instance?.LogWarning("Empty value passed to ImageView binding");
                return null;
            }

            var drawableResourceName = GetImageAssetName(stringValue);
            var assetStream = Application.Context.Assets?.Open(drawableResourceName);

            return assetStream;
        }

        private static string GetImageAssetName(string rawImage)
        {
            return rawImage.TrimStart('/');
        }
    }
}