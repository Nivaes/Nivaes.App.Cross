// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Binding.Bindings.Target.Construction
{
    using System;

    public static class MvxTargetBindingFactoryRegistryExtensions
    {
        public static void RegisterCustomBindingFactory<TView>(
            this IMvxTargetBindingFactoryRegistry registry,
            string customName,
            Func<TView, IMvxTargetBinding> creator)
            where TView : class
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            registry.RegisterFactory(new MvxCustomBindingFactory<TView>(customName, creator));
        }

        public static void RegisterPropertyInfoBindingFactory(this IMvxTargetBindingFactoryRegistry registry,
                                                              Type bindingType, Type targetType, string targetName)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(bindingType, targetType, targetName));
        }
    }
}
