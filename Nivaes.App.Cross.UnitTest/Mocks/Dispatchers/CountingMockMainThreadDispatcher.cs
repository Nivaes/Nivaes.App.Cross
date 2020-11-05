// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.UnitTest.Mocks.Dispatchers
{
    using System;
    using System.Threading.Tasks;
    using MvvmCross.Base;

    public class CountingMockMainThreadDispatcher
        : MvxMainThreadDispatcher
    {
        public int Count { get; set; }

        public override bool IsOnMainThread => true;

        public override ValueTask<bool> ExecuteOnMainThread(Action action, bool maskExceptions = true)
        {

            ExceptionMaskedAction(action, maskExceptions);
            Count++;

            return new ValueTask<bool>(true);
        }

        public override async ValueTask<bool> ExecuteOnMainThreadAsync(Func<ValueTask<bool>> action, bool maskExceptions = true)
        {
            await ExceptionMaskedActionAsync(action, maskExceptions).ConfigureAwait(false);
            Count++;

            return true;
        }
    }
}
