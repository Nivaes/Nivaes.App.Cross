// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Navigation;

    public interface IMvxViewModelLocator
    {
        ValueTask<IMvxViewModel> Load(Type viewModelType, IMvxBundle? parameterValues,
            IMvxBundle? savedState, IMvxNavigateEventArgs? navigationArgs = null);

        ValueTask<IMvxViewModel<TParameter>> Load<TParameter>(Type viewModelType,
            TParameter param, IMvxBundle? parameterValues, IMvxBundle? savedState, IMvxNavigateEventArgs? navigationArgs = null);

        ValueTask<IMvxViewModel> Reload(IMvxViewModel viewModel,
            IMvxBundle? parameterValues, IMvxBundle? savedState, IMvxNavigateEventArgs? navigationArgs = null);

        ValueTask<IMvxViewModel<TParameter>> Reload<TParameter>(IMvxViewModel<TParameter> viewModel,
            TParameter param, IMvxBundle? parameterValues, IMvxBundle? savedState, IMvxNavigateEventArgs? navigationArgs = null);
    }
}
