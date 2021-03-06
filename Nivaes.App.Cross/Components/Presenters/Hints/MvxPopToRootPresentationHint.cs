﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Presenters
{
    using System.Collections.Generic;
    using Nivaes.App.Cross.ViewModels;

    public class MvxPopToRootPresentationHint
        : MvxPresentationHint
    {
        public MvxPopToRootPresentationHint(bool animated = true)
            : base()
        {
            Animated = animated;
        }

        public MvxPopToRootPresentationHint(MvxBundle body, bool animated = true)
            : base(body)
        {
            Animated = animated;
        }

        public MvxPopToRootPresentationHint(IDictionary<string, string> hints, bool animated = true)
            : this(new MvxBundle(hints), animated)
        {
        }

        public bool Animated { get; set; }
    }
}
