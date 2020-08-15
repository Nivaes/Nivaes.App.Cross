// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using MvvmCross.Base;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Converters;
using MvvmCross.Exceptions;
using MvvmCross.IoC;

namespace MvvmCross.Binding.Bindings
{
    public class MvxFullBinding
        : MvxBinding, IMvxUpdateableBinding
    {
        private IMvxSourceStepFactory SourceStepFactory => MvxBindingSingletonCache.Instance.SourceStepFactory;

        private IMvxTargetBindingFactory TargetBindingFactory => MvxBindingSingletonCache.Instance.TargetBindingFactory;

        private readonly MvxBindingDescription mBindingDescription;
        private IMvxSourceStep? mSourceStep;
        private IMvxTargetBinding? mTargetBinding;
        private readonly object mTargetLocker = new object();

        private object? mDataContext;
        private EventHandler? mSourceBindingOnChanged;
        private EventHandler<MvxTargetChangedEventArgs> mTargetBindingOnValueChanged;

        private object mDefaultTargetValue;
        private CancellationTokenSource mCancelSource = new CancellationTokenSource();
        private IMvxMainThreadAsyncDispatcher mDispatcher => MvxBindingSingletonCache.Instance.MainThreadDispatcher;

        public object? DataContext
        {
            get => mDataContext;
            set
            {
                if (mDataContext == value)
                    return;
                mDataContext = value;

                if (mSourceStep != null)
                    mSourceStep.DataContext = value;

                UpdateTargetOnBind();
            }
        }

        public MvxFullBinding(MvxBindingRequest bindingRequest)
        {
            mBindingDescription = bindingRequest.Description;
            CreateTargetBinding(bindingRequest.Target);
            CreateSourceBinding(bindingRequest.Source);
        }

        protected virtual void ClearSourceBinding()
        {
            if (mSourceStep != null)
            {
                if (mSourceBindingOnChanged != null)
                {
                    mSourceStep.Changed -= mSourceBindingOnChanged;
                    mSourceBindingOnChanged = null;
                }

                mSourceStep.Dispose();
                mSourceStep = null;
            }
        }

        private void CreateSourceBinding(object source)
        {
            // NOTE: We do not call the setter for DataContext here because we are
            // setting up the sourceStep.
            // If that method is updated we will need to make sure that this method
            // does the right thing.
            mDataContext = source;
            mSourceStep = SourceStepFactory.Create(mBindingDescription.Source);
            mSourceStep.TargetType = mTargetBinding.TargetType;
            mSourceStep.DataContext = source;

            if (NeedToObserveSourceChanges)
            {
                mSourceBindingOnChanged = (sender, args) =>
                {
                    //Capture the cancel token first
                    var cancel = mCancelSource.Token;
                    //GetValue can now be executed in a worker thread. Is it the responsibility of the caller to switch threads, or ours ?
                    //As the source is the viewmodel, i suppose it is the responsibility of the caller.
                    var value = mSourceStep.GetValue();
                    UpdateTargetFromSource(value, cancel);
                };
                mSourceStep.Changed += mSourceBindingOnChanged;
            }

            UpdateTargetOnBind();
        }

        private void UpdateTargetOnBind()
        {
            if (NeedToUpdateTargetOnBind && mSourceStep != null)
            {
                mCancelSource.Cancel();
                mCancelSource = new CancellationTokenSource();
                var cancel = mCancelSource.Token;

                try
                {
                    var currentValue = mSourceStep.GetValue();
                    UpdateTargetFromSource(currentValue, cancel);
                }
                catch (Exception exception)
                {
                    MvxBindingLog.Trace("Exception masked in UpdateTargetOnBind {0}", exception.ToLongString());
                }
            }
        }

        protected virtual void ClearTargetBinding()
        {
            lock (mTargetLocker)
            {
                if (mTargetBinding != null)
                {
                    if (mTargetBindingOnValueChanged != null)
                    {
                        mTargetBinding.ValueChanged -= mTargetBindingOnValueChanged;
                        mTargetBindingOnValueChanged = null;
                    }

                    mTargetBinding.Dispose();
                    mTargetBinding = null;
                }
            }
        }

        private void CreateTargetBinding(object target)
        {
            mTargetBinding = TargetBindingFactory.CreateBinding(target, mBindingDescription.TargetName);

            if (mTargetBinding == null)
            {
                MvxBindingLog.Warning("Failed to create target binding for {0}", mBindingDescription.ToString());
                mTargetBinding = new MvxNullTargetBinding();
            }

            if (NeedToObserveTargetChanges)
            {
                mTargetBinding.SubscribeToEvents();
                mTargetBindingOnValueChanged = (sender, args) => UpdateSourceFromTarget(args.Value);
                mTargetBinding.ValueChanged += mTargetBindingOnValueChanged;
            }

            mDefaultTargetValue = mTargetBinding.TargetType.CreateDefault();
        }

        private async void UpdateTargetFromSource(object value, CancellationToken cancel)
        {
            if (value == MvxBindingConstant.DoNothing || cancel.IsCancellationRequested)
                return;

            if (value == MvxBindingConstant.UnsetValue)
                value = mDefaultTargetValue;

            await mDispatcher.ExecuteOnMainThreadAsync(() =>
            {
                if (cancel.IsCancellationRequested)
                    return;

                try
                {
                    lock (mTargetLocker)
                    {
                        mTargetBinding?.SetValue(value);
                    }
                }
                catch (Exception exception)
                {
                    MvxBindingLog.Error(
                        "Problem seen during binding execution for {0} - problem {1}",
                        mBindingDescription.ToString(),
                        exception.ToLongString());
                }
            }).ConfigureAwait(false);
        }

        private void UpdateSourceFromTarget(object value)
        {
            if (value == MvxBindingConstant.DoNothing)
                return;

            if (value == MvxBindingConstant.UnsetValue)
                return;

            try
            {
                mSourceStep.SetValue(value);
            }
            catch (Exception exception)
            {
                MvxBindingLog.Error(
                    "Problem seen during binding execution for {0} - problem {1}",
                    mBindingDescription.ToString(),
                    exception.ToLongString());
            }
        }

        protected bool NeedToObserveSourceChanges
        {
            get
            {
                var mode = ActualBindingMode;
                return mode.RequireSourceObservation();
            }
        }

        protected bool NeedToObserveTargetChanges
        {
            get
            {
                var mode = ActualBindingMode;
                return mode.RequiresTargetObservation();
            }
        }

        protected bool NeedToUpdateTargetOnBind
        {
            get
            {
                var bindingMode = ActualBindingMode;
                return bindingMode.RequireTargetUpdateOnFirstBind();
            }
        }

        protected MvxBindingMode ActualBindingMode
        {
            get
            {
                var mode = mBindingDescription.Mode;
                if (mode == MvxBindingMode.Default && mTargetBinding != null)
                    mode = mTargetBinding.DefaultMode;
                return mode;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                ClearTargetBinding();
                ClearSourceBinding();
            }
        }
    }
}
