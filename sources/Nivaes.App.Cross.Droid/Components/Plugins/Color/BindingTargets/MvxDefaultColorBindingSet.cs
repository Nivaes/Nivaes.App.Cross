using Android.Views;

namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Runtime;
    using Microsoft.Extensions.Logging;
    using MvvmCross.IoC;
    using MvvmCross.Plugin.Color.Platforms.Android.Binding;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Droid;

    public class MvxDefaultColorBindingSet
    {
        public void RegisterBindings(IMvxIoCProvider provider)
        {
            if (!provider.TryResolve(out ICrossTargetBindingFactoryRegistry registry) || registry == null)
            {
                MvxAndroidLog.Instance.Log(LogLevel.Warning,
                    "No binding registry available - so color bindings will not be used");
                return;
            }

            registry.RegisterFactory(new CrossCustomBindingFactory<View>(
                MvxAndroidColorPropertyBinding.View_BackgroundColor,
                view => new MvxViewBackgroundColorBinding(view)));

            registry.RegisterFactory(new CrossCustomBindingFactory<TextView>(
                MvxAndroidColorPropertyBinding.TextView_TextColor,
                textView => new MvxTextViewTextColorBinding(textView)));
        }
    }
}
