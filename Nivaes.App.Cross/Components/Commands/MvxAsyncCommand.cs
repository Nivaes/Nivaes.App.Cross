// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Logging;

    public abstract class MvxAsyncCommandBase
        : MvxCommandBase, IDisposable
    {
        private readonly object mSyncRoot = new object();
        private readonly bool mAllowConcurrentExecutions;
        private CancellationTokenSource? mCancellationTokenSource;
        private int mConcurrentExecutions;

        protected MvxAsyncCommandBase(bool allowConcurrentExecutions = false)
        {
            mAllowConcurrentExecutions = allowConcurrentExecutions;
        }

        public bool IsRunning => mConcurrentExecutions > 0;

        protected CancellationToken CancelToken
        {
            get
            {
                if (mCancellationTokenSource == null)
                    mCancellationTokenSource = new CancellationTokenSource();

                return mCancellationTokenSource.Token;
            }
        }

        protected abstract bool CanExecuteImpl(object? parameter);

        protected abstract ValueTask<bool> ExecuteAsyncImpl(object? parameter);

        public void Cancel()
        {
            lock (mSyncRoot)
            {
                if (mCancellationTokenSource == null)
                {
                    MvxLog.Instance?.Warn("MvxAsyncCommand : Attempt to cancel a task that is not running");
                }
                else
                {
                    mCancellationTokenSource.Cancel();
                }
            }
        }

        public bool CanExecute()
        {
            return CanExecute(null);
        }

        public bool CanExecute(object? parameter)
        {
            if (!mAllowConcurrentExecutions && IsRunning)
                return false;
            else
                return CanExecuteImpl(parameter);
        }

        public void Execute(object? parameter)
        {
            try
            {
                _ = ExecuteAsync(parameter, true).AsTask();
            }
            catch (Exception e)
            {
                MvxLog.Instance?.Error("MvxAsyncCommand : exception executing task : ", e);
                throw;
            }
        }

        public void Execute()
        {
            Execute(null);
        }

        protected ValueTask ExecuteAsync(object? parameter, bool hideCanceledException)
        {
            if (CanExecuteImpl(parameter))
            {
                return ExecuteConcurrentAsync(parameter, hideCanceledException);
            }
            else
            {
                return new ValueTask();
            }
        }

        private async ValueTask ExecuteConcurrentAsync(object? parameter, bool hideCanceledException)
        {
            bool started = false;
            try
            {
                lock (mSyncRoot)
                {
                    if (mConcurrentExecutions == 0)
                    {
                        InitCancellationTokenSource();
                    }
                    else if (!mAllowConcurrentExecutions)
                    {
                        MvxLog.Instance?.Info("MvxAsyncCommand : execute ignored, already running.");
                        return;
                    }
                    mConcurrentExecutions++;
                    started = true;
                }

                if (!mAllowConcurrentExecutions)
                {
                    RaiseCanExecuteChanged();
                }

                if (!CancelToken.IsCancellationRequested)
                {
                    try
                    {
                        // With configure await false, the CanExecuteChanged raised in finally clause might run in another thread.
                        // This should not be an issue as long as ShouldAlwaysRaiseCECOnUserInterfaceThread is true.
                        await ExecuteAsyncImpl(parameter).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException e)
                    {
                        MvxLog.Instance?.Trace("MvxAsyncCommand : OperationCanceledException");
                        //Rethrow if the exception does not come from the current cancellation token
                        if (!hideCanceledException || e.CancellationToken != CancelToken)
                        {
                            throw;
                        }
                    }
                }
            }
            finally
            {
                if (started)
                {
                    lock (mSyncRoot)
                    {
                        mConcurrentExecutions--;
                        if (mConcurrentExecutions == 0)
                        {
                            ClearCancellationTokenSource();
                        }
                    }
                    if (!mAllowConcurrentExecutions)
                    {
                        RaiseCanExecuteChanged();
                    }
                }
            }
        }

        private void ClearCancellationTokenSource()
        {
            if (mCancellationTokenSource == null)
            {
                MvxLog.Instance?.Error("MvxAsyncCommand : Unexpected ClearCancellationTokenSource, no token available!");
            }
            else
            {
                mCancellationTokenSource.Dispose();
                mCancellationTokenSource = null;
            }
        }

        private void InitCancellationTokenSource()
        {
            if (mCancellationTokenSource != null)
            {
                MvxLog.Instance?.Error("MvxAsyncCommand : Unexpected InitCancellationTokenSource, a token is already available!");
            }
            mCancellationTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                mCancellationTokenSource?.Dispose();
            }
        }
    }

    public class MvxAsyncCommand
        : MvxAsyncCommandBase
        , IMvxAsyncCommand
    {
        private readonly Func<CancellationToken, ValueTask<bool>> mExecute;
        private readonly Func<bool>? mCanExecute;

        public MvxAsyncCommand(Func<ValueTask<bool>> execute, Func<bool>? canExecute = null, bool allowConcurrentExecutions = false)
           : base(allowConcurrentExecutions)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            mExecute = (_) => execute();
            mCanExecute = canExecute;
        }

        public MvxAsyncCommand(Func<CancellationToken, ValueTask<bool>> execute, Func<bool>? canExecute = null, bool allowConcurrentExecutions = false)
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

    public class MvxAsyncCommand<T>
        : MvxAsyncCommandBase
        , IMvxCommand, IMvxAsyncCommand<T>
    {
        private readonly Func<T, CancellationToken, ValueTask<bool>> mExecute;
        private readonly Func<T, bool>? mCanExecute;

        public MvxAsyncCommand(Func<T, ValueTask<bool>> execute, Func<T, bool>? canExecute = null, bool allowConcurrentExecutions = false)
           : base(allowConcurrentExecutions)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            mExecute = (p, _) => execute(p);
            mCanExecute = canExecute;
        }

        public MvxAsyncCommand(Func<T, CancellationToken, ValueTask<bool>> execute, Func<T, bool>? canExecute = null, bool allowConcurrentExecutions = false)
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
