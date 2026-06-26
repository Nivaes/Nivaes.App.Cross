using Android.Views;

namespace Nivaes.App.Cross.Droid
{
    using Microsoft.Extensions.DependencyInjection;
    using MvvmCross.IoC;
    using MvvmCross.Plugin.Color.Platforms.Android.Binding;
    using Nivaes.App.Cross;

    [Obsolete("", true)]
    public class MvxDefaultColorBindingSet
    {
        public void RegisterBindings(IMvxIoCProvider provider)
        {
            //if (!provider.TryResolve(out ICrossTargetBindingFactoryRegistry registry) || registry == null)
            //{
            //    MvxAndroidLog.Instance.Log(LogLevel.Warning,
            //        "No binding registry available - so color bindings will not be used");
            //    return;
            //}

            var registry = IPlatformApplication.Current!.Services.GetRequiredService<ICrossTargetBindingFactoryRegistry>();

            registry.RegisterFactory(new CrossCustomBindingFactory<View>(
                MvxAndroidColorPropertyBinding.View_BackgroundColor,
                view => new MvxViewBackgroundColorBinding(view)));

            registry.RegisterFactory(new CrossCustomBindingFactory<TextView>(
                MvxAndroidColorPropertyBinding.TextView_TextColor,
                textView => new MvxTextViewTextColorBinding(textView)));
        }
    }
}
