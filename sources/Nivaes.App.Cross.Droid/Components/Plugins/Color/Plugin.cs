// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.IoC;
using MvvmCross.Plugin.Color.Platforms.Android.BindingTargets;
using Nivaes.App.Cross;

namespace MvvmCross.Plugin.Color.Platforms.Android
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public sealed class Plugin : CrossBasePlugin
    {
        public override void Load(IMvxIoCProvider provider)
        {
            provider.RegisterSingleton<ICrossNativeColor>(new MvxAndroidColor());
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
