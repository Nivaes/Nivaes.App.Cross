// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Tizen.Presenters
{
    using System;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Presenters;

    public class MvxTizenViewPresenter
        : MvxAttributeViewPresenter, IMvxTizenViewPresenter
    {
        public override ValueTask<MvxBasePresentationAttribute> CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            return new ValueTask<MvxBasePresentationAttribute>((MvxBasePresentationAttribute)null!);
        }

        public override void RegisterAttributeTypes()
        {
        }
    }
}
