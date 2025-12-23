using Android.Webkit;

namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Binding;

    public class MvxWebViewHtmlTargetBinding(object target)
    : MvxAndroidTargetBinding(target)
    {
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(string);
        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWay;

        protected override void SetValueImpl(object target, object? value)
        {
            if (target is WebView webView && value is string html)
            {
                webView.LoadData(html, "text/html; charset=utf-8", "UTF-8");
            }
        }
    }
}