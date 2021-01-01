﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;

    /// <summary>
    /// Watches a task and raises property-changed notifications when the task completes.
    /// </summary>
    public sealed class MvxNotifyTask
        : INotifyPropertyChanged
    {
        private readonly Action<Exception>? mOnException;

        /// <summary>
        /// Initializes a task notifier watching the specified task.
        /// </summary>
        /// <param name="task">The task to watch.</param>
        /// <param name="onException">Callback to be run when an error happens</param>
        private MvxNotifyTask(Task task, Action<Exception>? onException)
        {
            Task = task;
            mOnException = onException;
            TaskCompleted = MonitorTaskAsync(task);
        }

        private async Task MonitorTaskAsync(Task task)
        {
            try
            {
                await Task.Yield();
                await task.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                mOnException?.Invoke(e);
            }
            finally
            {
                NotifyProperties(task);
            }
        }

        private void NotifyProperties(Task task)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged == null)
                return;

            if (task.IsCanceled)
            {
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get(nameof(Status)));
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get(nameof(IsCanceled)));
            }
            else if (task.IsFaulted)
            {
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get(nameof(Exception)));
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get(nameof(InnerException)));
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get(nameof(ErrorMessage)));
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get(nameof(Status)));
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get(nameof(IsFaulted)));
            }
            else
            {
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get(nameof(Status)));
                propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get(nameof(IsSuccessfullyCompleted)));
            }
            propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get(nameof(IsCompleted)));
            propertyChanged(this, PropertyChangedEventArgsCache.Instance.Get(nameof(IsNotCompleted)));
        }

        /// <summary>
        /// Gets the task being watched. This property never changes and is never <c>null</c>.
        /// </summary>
        public Task Task { get; }

        /// <summary>
        /// Gets a task that completes successfully when <see cref="Task"/> completes (successfully, faulted, or canceled). This property never changes and is never <c>null</c>.
        /// </summary>
        public Task TaskCompleted { get; }

        /// <summary>
        /// Gets the current task status. This property raises a notification when the task completes.
        /// </summary>
        public TaskStatus Status => Task.Status;

        /// <summary>
        /// Gets whether the task has completed. This property raises a notification when the value changes to <c>true</c>.
        /// </summary>
        public bool IsCompleted => Task.IsCompleted;

        /// <summary>
        /// Gets whether the task is busy (not completed). This property raises a notification when the value changes to <c>false</c>.
        /// </summary>
        public bool IsNotCompleted => !Task.IsCompleted;

        /// <summary>
        /// Gets whether the task has completed successfully. This property raises a notification when the value changes to <c>true</c>.
        /// </summary>
        public bool IsSuccessfullyCompleted => Task.Status == TaskStatus.RanToCompletion;

        /// <summary>
        /// Gets whether the task has been canceled. This property raises a notification only if the task is canceled (i.e., if the value changes to <c>true</c>).
        /// </summary>
        public bool IsCanceled => Task.IsCanceled;

        /// <summary>
        /// Gets whether the task has faulted. This property raises a notification only if the task faults (i.e., if the value changes to <c>true</c>).
        /// </summary>
        public bool IsFaulted => Task.IsFaulted;

        /// <summary>
        /// Gets the wrapped faulting exception for the task. Returns <c>null</c> if the task is not faulted. This property raises a notification only if the task faults (i.e., if the value changes to non-<c>null</c>).
        /// </summary>
        public AggregateException Exception => Task.Exception;

        /// <summary>
        /// Gets the original faulting exception for the task. Returns <c>null</c> if the task is not faulted. This property raises a notification only if the task faults (i.e., if the value changes to non-<c>null</c>).
        /// </summary>
        public Exception? InnerException => Exception?.InnerException;

        /// <summary>
        /// Gets the error message for the original faulting exception for the task. Returns <c>null</c> if the task is not faulted. This property raises a notification only if the task faults (i.e., if the value changes to non-<c>null</c>).
        /// </summary>
        public string? ErrorMessage => InnerException?.Message;

        /// <summary>
        /// Event that notifies listeners of property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Creates a new task notifier watching the specified task.
        /// </summary>
        /// <param name="task">The task to watch.</param>
        /// <param name="onException">Callback to be run when an error happens</param>
        public static MvxNotifyTask Create(Task task, Action<Exception>? onException = null)
        {
            return new MvxNotifyTask(task, onException);
        }

        /// <summary>
        /// Creates a new task notifier watching the specified task.
        /// </summary>
        /// <typeparam name="TResult">The type of the task result.</typeparam>
        /// <param name="task">The task to watch.</param>
        /// <param name="defaultResult">The default "result" value for the task while it is not yet complete.</param>
        /// <param name="onException">Callback to be run when an error happens</param>
        public static MvxNotifyTask<TResult> Create<TResult>(Task<TResult> task, TResult? defaultResult = default, Action<Exception>? onException = null)
        {
            return new MvxNotifyTask<TResult>(task, defaultResult, onException);
        }

        /// <summary>
        /// Executes the specified asynchronous code and creates a new task notifier watching the returned task.
        /// </summary>
        /// <param name="asyncAction">The asynchronous code to execute.</param>
        /// <param name="onException">Callback to be run when an error happens</param>
        public static MvxNotifyTask Create(Func<Task> asyncAction, Action<Exception>? onException = null)
        {
            if (asyncAction == null) throw new ArgumentNullException(nameof(asyncAction));

            return Create(asyncAction(), onException);
        }

        /// <summary>
        /// Executes the specified asynchronous code and creates a new task notifier watching the returned task.
        /// </summary>
        /// <param name="asyncAction">The asynchronous code to execute.</param>
        /// <param name="defaultResult">The default "result" value for the task while it is not yet complete.</param>
        /// <param name="onException">Callback to be run when an error happens</param>
        public static MvxNotifyTask<TResult> Create<TResult>(Func<Task<TResult>> asyncAction, TResult defaultResult = default, Action<Exception>? onException = null)
        {
            if (asyncAction == null) throw new ArgumentNullException(nameof(asyncAction));

            return Create(asyncAction(), defaultResult, onException);
        }
    }
}
