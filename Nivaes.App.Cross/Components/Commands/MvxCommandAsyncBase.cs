// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Logging;

    public abstract class MvxCommandAsyncBase
        : MvxCommandBase, IDisposable
    {
        private readonly object mSyncRoot = new object();
        private readonly bool mAllowConcurrentExecutions;
        private CancellationTokenSource? mCancellationTokenSource;
        private int mConcurrentExecutions;

        protected MvxCommandAsyncBase(bool allowConcurrentExecutions = false)
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
}
