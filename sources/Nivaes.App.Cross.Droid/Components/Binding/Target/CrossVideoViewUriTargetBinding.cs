namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using AndroidUri = Android.Net.Uri;

    public class CrossVideoViewUriTargetBinding(VideoView videoView) : CrossAndroidTargetBinding(videoView)
    {
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(string);

        protected override void SetValueImpl(object target, object? value)
        {
            if (target is VideoView view && value is string uri && !string.IsNullOrWhiteSpace(uri))
            {
                view.SetVideoURI(AndroidUri.Parse(uri));
            }
        }
    }
}