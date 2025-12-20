// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Plugin;
using MvvmCross.Plugin.Color;
using MvvmCross.Plugin.Color.Platforms.Android;
using MvvmCross.Plugin.Color.Platforms.Android.BindingTargets;
using MvvmCross.UI;

namespace Nivaes.App.Cross.Droid.Components.Plugins
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public sealed class Plugin : BasePlugin
    {
        public override void Load(IMvxIoCProvider provider)
        {
            provider.RegisterSingleton<IMvxNativeColor>(new MvxAndroidColor());
            RegisterDefaultBindings(provider);
            base.Load(provider);
        }

        private static void RegisterDefaultBindings(IMvxIoCProvider provider)
        {
            var helper = new MvxDefaultColorBindingSet();
            helper.RegisterBindings(provider);
        }
    }
}
