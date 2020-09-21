// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Presenters
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MvvmCross.ViewModels;

    public abstract class MvxViewPresenter
        : IMvxViewPresenter
    {
        private readonly Dictionary<Type, Func<MvxPresentationHint, Task<bool>>> _presentationHintHandlers = new Dictionary<Type, Func<MvxPresentationHint, Task<bool>>>();

        public void AddPresentationHintHandler<THint>(Func<THint, Task<bool>> action) where THint : MvxPresentationHint
        {
            _presentationHintHandlers[typeof(THint)] = hint => action((THint)hint);
        }

        protected Task<bool> HandlePresentationChange(MvxPresentationHint hint)
        {

            if (_presentationHintHandlers.TryGetValue(hint.GetType(), out Func<MvxPresentationHint, Task<bool>> handler))
            {
                return handler(hint);
            }

            return Task.FromResult(false);
        }

        public abstract ValueTask<bool> Show(MvxViewModelRequest request);

        public abstract ValueTask<bool> ChangePresentation(MvxPresentationHint hint);

        public abstract ValueTask<bool> Close(IMvxViewModel viewModel);
    }
}
