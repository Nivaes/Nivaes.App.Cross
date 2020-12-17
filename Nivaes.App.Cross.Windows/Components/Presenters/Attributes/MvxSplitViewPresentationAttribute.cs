// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Uap.Presenters.Attributes
{
    using System.Runtime.Versioning;
    using Nivaes.App.Cross.Presenters;

    public class MvxSplitViewPresentationAttribute
        : MvxBasePresentationAttribute
    {

        public MvxSplitViewPresentationAttribute() : this(SplitPanePosition.Content)
        {

        }

        public MvxSplitViewPresentationAttribute(SplitPanePosition position)
        {
            Position = position;
        }

        [SupportedOSPlatform("windows")]
        public SplitPanePosition Position { get; set; }
    }

    public enum SplitPanePosition
    {
        Pane,
        Content
    }

}
