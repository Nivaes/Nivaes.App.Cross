// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Tizen.Views
{
    using System;
    using System.Threading.Tasks;
    using MvvmCross.Base;

    public class MvxTizenMainThreadDispatcher
        : MvxMainThreadDispatcher
    {
        public override bool IsOnMainThread => true;

        public override ValueTask<bool> ExecuteOnMainThread(Action action, bool maskExceptions = true)
        {
            //TODO: implement
            ExceptionMaskedAction(action, maskExceptions);

            return new ValueTask<bool>(true);
        }

        public override ValueTask<bool> ExecuteOnMainThreadAsync(Func<ValueTask<bool>> action, bool maskExceptions = true)
        {
            return ExceptionMaskedActionAsync(action, maskExceptions);
        }
    }
}
