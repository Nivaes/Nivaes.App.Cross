﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Presenters
{
    using System;
    using System.Collections.Generic;
    using Nivaes.App.Cross.ViewModels;

    public class MvxPopPresentationHint
        : MvxPresentationHint
    {
        public MvxPopPresentationHint(Type viewModelToPopTo, bool animated = false)
            : base()
        {
            ViewModelToPopTo = viewModelToPopTo;
            Animated = animated;
        }

        public MvxPopPresentationHint(MvxBundle body, Type viewModelToPopTo, bool animated = true)
            : base(body)
        {
            ViewModelToPopTo = viewModelToPopTo;
            Animated = animated;
        }

        public MvxPopPresentationHint(IDictionary<string, string> hints, Type viewModelToPopTo, bool animated = true)
            : this(new MvxBundle(hints), viewModelToPopTo, animated)
        {
        }

        public Type ViewModelToPopTo { get; private set; }

        public bool Animated { get; set; }
    }
}
