// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Presenters
{
    using System.Collections.Generic;
    using MvvmCross.ViewModels;

    public class MvxClosePresentationHint
        : MvxPresentationHint
    {
        public MvxClosePresentationHint(IMvxViewModel viewModelToClose) : base()
        {
            ViewModelToClose = viewModelToClose;
        }

        public MvxClosePresentationHint(IMvxViewModel viewModelToClose, MvxBundle body) : base(body)
        {
            ViewModelToClose = viewModelToClose;
        }

        public MvxClosePresentationHint(IMvxViewModel viewModelToClose, IDictionary<string, string> hints) : this(viewModelToClose, new MvxBundle(hints))
        {
        }

        public IMvxViewModel ViewModelToClose { get; private set; }
    }
}
