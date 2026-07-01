using Android.Webkit;

namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;

    public class MvxWebViewUriTargetBinding(WebView webView)
    : MvxAndroidTargetBinding(webView)
    {
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(string);
        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWay;

        protected override void SetValueImpl(object target, object? value)
        {
            if (target is WebView view && value is string uri)
            {
                view.LoadUrl(uri);
            }
        }
    }
}