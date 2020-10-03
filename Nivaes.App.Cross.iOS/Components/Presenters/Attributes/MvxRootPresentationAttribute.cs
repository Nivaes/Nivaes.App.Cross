// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Presenters
{
    using UIKit;

    public class MvxRootPresentationAttribute
        : MvxBasePresentationAttribute
    {
        public static float DefaultAnimationDuration = 1.0f;

        public static bool DefaultWrapInNavigationController = false;

        public static UIViewAnimationOptions DefaultAnimationOptions = UIViewAnimationOptions.TransitionNone;

        public float AnimationDuration { get; set; } = DefaultAnimationDuration;

        public UIViewAnimationOptions AnimationOptions { get; set; } = DefaultAnimationOptions;

        public bool WrapInNavigationController { get; set; } = DefaultWrapInNavigationController;
    }
}
