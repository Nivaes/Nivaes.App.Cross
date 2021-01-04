// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Uap.Binding
{
    using System;
    using System.Collections.Generic;
    using Microsoft.UI.Xaml;
    using MvvmCross.Binding.Bindings;
    using Nivaes.App.Cross.Logging;

    public abstract class MvxBindingCreator
        : IMvxBindingCreator
    {
        public void CreateBindings(object sender, DependencyPropertyChangedEventArgs args,
                                   Func<string, IEnumerable<MvxBindingDescription>> parseBindingDescriptions)
        {
            if (parseBindingDescriptions == null) throw new ArgumentNullException(nameof(parseBindingDescriptions));

            var attachedObject = sender as FrameworkElement;
            if (attachedObject == null)
            {
                //MvxLog.Instance?.Warn("Null attached FrameworkElement seen in Bi.nd binding");
                return;
            }

            var text = args.NewValue as string;
            if (string.IsNullOrEmpty(text))
                return;

            var bindingDescriptions = parseBindingDescriptions(text);
            if (bindingDescriptions == null)
                return;

            ApplyBindings(attachedObject, bindingDescriptions);
        }

        protected abstract void ApplyBindings(FrameworkElement attachedObject,
                                              IEnumerable<MvxBindingDescription> bindingDescriptions);
    }
}
