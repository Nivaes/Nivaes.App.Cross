// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Views;

namespace MvvmCross.Platforms.Android.Binding.Target
{
    using System;
    using System.Windows.Input;
    using MvvmCross.Binding;
    using MvvmCross.WeakSubscription;

    public class MvxViewClickBinding
        : MvxAndroidTargetBinding
    {
        private ICommand? mCommand;
        private IDisposable? mClickSubscription;
        private IDisposable? mCanExecuteSubscription;
        private readonly EventHandler<EventArgs> mCanExecuteEventHandler;

        protected View View => (View)Target;

        public MvxViewClickBinding(View view)
            : base(view)
        {
            mCanExecuteEventHandler = OnCanExecuteChanged;
            mClickSubscription = view.WeakSubscribe(nameof(view.Click), ViewOnClick);
        }

        private void ViewOnClick(object sender, EventArgs args)
        {
            if (mCommand == null)
                return;

            if (!mCommand.CanExecute(null))
                return;

            mCommand.Execute(null);
        }

        protected override void SetValueImpl(object target, object value)
        {
            mCanExecuteSubscription?.Dispose();
            mCanExecuteSubscription = null;

            mCommand = value as ICommand;
            if (mCommand != null)
            {
                mCanExecuteSubscription = mCommand.WeakSubscribe(mCanExecuteEventHandler);
            }
            RefreshEnabledState();
        }

        private void RefreshEnabledState()
        {
            var view = View;
            if (view == null)
                return;

            var shouldBeEnabled = false;
            if (mCommand != null)
            {
                shouldBeEnabled = mCommand.CanExecute(null);
            }
            view.Enabled = shouldBeEnabled;
        }

        private void OnCanExecuteChanged(object sender, EventArgs e)
        {
            RefreshEnabledState();
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        public override Type TargetType => typeof(ICommand);

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                mClickSubscription?.Dispose();
                mClickSubscription = null;

                mCanExecuteSubscription?.Dispose();
                mCanExecuteSubscription = null;
            }
            base.Dispose(isDisposing);
        }
    }
}
