// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Presenters
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.ViewModels;

    public abstract class MvxViewPresenter
        : IMvxViewPresenter
    {
        private readonly Dictionary<Type, Func<MvxPresentationHint, ValueTask<bool>>> mPresentationHintHandlers = new Dictionary<Type, Func<MvxPresentationHint, ValueTask<bool>>>();

        public void AddPresentationHintHandler<THint>(Func<THint, ValueTask<bool>> action)
            where THint : MvxPresentationHint
        {
            mPresentationHintHandlers[typeof(THint)] = hint => action((THint)hint);
        }

        protected ValueTask<bool> HandlePresentationChange(MvxPresentationHint hint)
        {
            if (hint == null) throw new ArgumentNullException(nameof(hint));

            if (mPresentationHintHandlers.TryGetValue(hint.GetType(), out Func<MvxPresentationHint, ValueTask<bool>> handler))
            {
                return handler(hint);
            }

            return new ValueTask<bool>(false);
        }

        public abstract ValueTask<bool> Show(MvxViewModelRequest request);

        public abstract ValueTask<bool> ChangePresentation(MvxPresentationHint hint);

        public abstract ValueTask<bool> Close(IMvxViewModel viewModel);
    }
}
