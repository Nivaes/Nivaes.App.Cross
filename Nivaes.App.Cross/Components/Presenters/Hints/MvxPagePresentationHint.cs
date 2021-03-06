﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Presenters
{
    using System;
    using System.Collections.Generic;
    using Nivaes.App.Cross.ViewModels;

    public class MvxPagePresentationHint
        : MvxPresentationHint
    {
        public MvxPagePresentationHint(Type viewModel)
            : base()
        {
            ViewModel = viewModel;
        }

        public MvxPagePresentationHint(Type viewModel, MvxBundle body)
            : base(body)
        {
            ViewModel = viewModel;
        }

        public MvxPagePresentationHint(Type viewModel, IDictionary<string, string> hints)
            : this(viewModel, new MvxBundle(hints))
        {
        }

        public Type ViewModel { get; }
    }
}
