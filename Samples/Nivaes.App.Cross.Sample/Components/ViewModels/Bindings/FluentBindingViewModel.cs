// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;

    public class FluentBindingViewModel
        : MvxNavigationViewModel
    {
        private bool mBindingsEnabled = true;

        public FluentBindingViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            ClearBindingsCommand = new MvxCommand(ClearBindings);
        }

        public IMvxCommand ClearBindingsCommand { get; }

        public MvxInteraction<bool> ClearBindingInteraction { get; } = new MvxInteraction<bool>();

        private string mTextValue = string.Empty;
        public string TextValue
        {
            get => mTextValue;
            set => SetProperty(ref mTextValue, value);
        }

        private void ClearBindings()
        {
            mBindingsEnabled = !mBindingsEnabled;
            ClearBindingInteraction.Raise(mBindingsEnabled);
        }
    }
}
