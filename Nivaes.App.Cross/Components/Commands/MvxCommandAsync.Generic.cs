// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class MvxCommandAsync<T>
        : MvxCommandAsyncBase
        , IMvxCommand, IMvxCommandAsync<T>
    {
        private readonly Func<T, CancellationToken, ValueTask<bool>> mExecute;
        private readonly Func<T, bool>? mCanExecute;

        public MvxCommandAsync(Func<T, ValueTask<bool>> execute, Func<T, bool>? canExecute = null, bool allowConcurrentExecutions = false)
           : base(allowConcurrentExecutions)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            mExecute = (p, _) => execute(p);
            mCanExecute = canExecute;
        }

        public MvxCommandAsync(Func<T, CancellationToken, ValueTask<bool>> execute, Func<T, bool>? canExecute = null, bool allowConcurrentExecutions = false)
            : base(allowConcurrentExecutions)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            mExecute = execute ?? throw new ArgumentNullException(nameof(execute));
            mCanExecute = canExecute;
        }

        public ValueTask ExecuteAsync(T parameter)
            => ExecuteAsync(parameter, false);

        public void Execute(T parameter)
            => base.Execute(parameter);

        public bool CanExecute(T parameter)
            => base.CanExecute(parameter);

        protected override bool CanExecuteImpl(object? parameter)
            => mCanExecute == null || mCanExecute((T)typeof(T).MakeSafeValueCore(parameter));

        protected override ValueTask<bool> ExecuteAsyncImpl(object? parameter)
            => mExecute((T)typeof(T).MakeSafeValueCore(parameter), CancelToken);
    }
}
