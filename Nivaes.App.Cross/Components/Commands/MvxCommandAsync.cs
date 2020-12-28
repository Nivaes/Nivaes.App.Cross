// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class MvxCommandAsync
        : MvxCommandAsyncBase
        , IMvxCommandAsync
    {
        private readonly Func<CancellationToken, ValueTask<bool>> mExecute;
        private readonly Func<bool>? mCanExecute;

        public MvxCommandAsync(Func<ValueTask<bool>> execute, Func<bool>? canExecute = null, bool allowConcurrentExecutions = false)
           : base(allowConcurrentExecutions)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            mExecute = (_) => execute();
            mCanExecute = canExecute;
        }

        public MvxCommandAsync(Func<CancellationToken, ValueTask<bool>> execute, Func<bool>? canExecute = null, bool allowConcurrentExecutions = false)
           : base(allowConcurrentExecutions)
        {
            mExecute = execute ?? throw new ArgumentNullException(nameof(execute));
            mCanExecute = canExecute;
        }

        protected override bool CanExecuteImpl(object? parameter)
        {
            return mCanExecute == null || mCanExecute();
        }

        protected override ValueTask<bool> ExecuteAsyncImpl(object? parameter)
        {
            return mExecute(CancelToken);
        }

        public ValueTask ExecuteAsync(object? parameter = null)
        {
            return base.ExecuteAsync(parameter, false);
        }
    }
}
