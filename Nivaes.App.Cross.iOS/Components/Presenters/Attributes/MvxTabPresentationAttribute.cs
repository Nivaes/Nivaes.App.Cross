// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Presenters
{
    public class MvxTabPresentationAttribute
        : MvxBasePresentationAttribute
    {
        public string? TabName { get; set; }

        public string? TabIconName { get; set; }

        public string? TabSelectedIconName { get; set; }

        public static bool DefaultWrapInNavigationController = true;
        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;

        public string? TabAccessibilityIdentifier { get; set; }
    }
}
