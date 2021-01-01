// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Android.Binding.Target
{
    using System;
    using AndroidX.Preference;
    using MvvmCross.Binding;
    using MvvmCross.Platforms.Android.WeakSubscription;
    using Nivaes.App.Cross.Logging;

    public class MvxPreferenceValueTargetBinding
        : MvxAndroidTargetBinding
    {
        private IDisposable? mSubscription;

        public MvxPreferenceValueTargetBinding(Preference preference)
            : base(preference)
        {
        }

        public Preference Preference => Target as Preference;

        public override Type TargetType => typeof(Preference);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            mSubscription = Preference.WeakSubscribe<Preference, Preference.PreferenceChangeEventArgs>(
                nameof(Preference.PreferenceChange),
                HandlePreferenceChange);
        }

        protected void HandlePreferenceChange(object sender, Preference.PreferenceChangeEventArgs e)
        {
            if (e.Preference == Preference)
            {
                FireValueChanged(e.NewValue);
                e.Handled = true;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                mSubscription?.Dispose();
                mSubscription = null;
            }

            base.Dispose(isDisposing);
        }

        protected override void SetValueImpl(object target, object value)
        {
            MvxBindingLog.Instance.Warn("SetValueImpl called on generic Preference target");
        }
    }
}
