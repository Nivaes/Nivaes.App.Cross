// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using MvvmCross.Logging;
using UIKit;

namespace Nivaes.App.Cross.UIKit
{
#nullable enable
    public static class UIViewControllerExtensions
    {
        public static ICrossIosView? GetIMvxIosView(this UIViewController? viewController)
        {
            if (viewController is ICrossIosView iosView)
            {
                return iosView;
            }

            CrossLogHost.Default?.Log(LogLevel.Warning, "Could not get IMvxIosView from ViewController {viewControllerName}",
                viewController?.GetType().Name);
            return null;
        }
    }
#nullable restore
}
