// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Mac.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AppKit;
    using Foundation;
    using MvvmCross.Core;
    using MvvmCross.Platforms.Mac.Views.Base;
    using MvvmCross.Views;
    using Nivaes.App.Cross.ViewModels;

    internal static class MvxSegueExtensions
    {
        internal static Type? GetViewModelType(this IMvxView view)
        {
            var viewType = view.GetType();
            var props = viewType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var prop = props.FirstOrDefault(p => p.Name == "ViewModel");
            return prop?.PropertyType;
        }

        internal static void ViewModelRequestForSegue(this IMvxEventSourceViewController self, NSStoryboardSegue segue, NSObject sender)
        {
            var parameterValues = self is not IMvxMacViewSegue view ? null : view.PrepareViewModelParametersForSegue(segue, sender);

            if (parameterValues is IMvxBundle mvxBundle)
                self.ViewModelRequestForSegueImpl(segue, mvxBundle);
            else if (parameterValues is IDictionary<string?, string>)
                self.ViewModelRequestForSegueImpl(segue, (IDictionary<string, string>)parameterValues);
            else
                self.ViewModelRequestForSegueImpl(segue, parameterValues);
        }

        private static void ViewModelRequestForSegueImpl(this IMvxEventSourceViewController self, NSStoryboardSegue segue, object? parameterValuesObject)
        {
            self.ViewModelRequestForSegueImpl(segue, parameterValuesObject!.ToSimplePropertyDictionary());
        }

        private static void ViewModelRequestForSegueImpl(this IMvxEventSourceViewController self, NSStoryboardSegue segue, IDictionary<string, string> parameterValues)
        {
            self.ViewModelRequestForSegueImpl(segue, new MvxBundle(parameterValues));
        }

        private static void ViewModelRequestForSegueImpl(this IMvxEventSourceViewController _, NSStoryboardSegue segue, IMvxBundle? parameterBundle = null)
        {
            if (segue.DestinationController is IMvxMacView view && view.Request == null)
            {
                var type = view.GetViewModelType();
                if (type != null)
                {
                    view.Request = new MvxViewModelRequest(type, parameterBundle, null);
                }
            }
        }
    }
}
