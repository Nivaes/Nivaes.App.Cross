// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross.Commands;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;

    public class DictionaryBindingViewModel
        : MvxNavigationViewModel
    {
        private int mValue;
        public int Value
        {
            get => mValue;
            set => SetProperty(ref mValue, value);
        }

        private IMvxCommandAsync? mCloseCommand;
        public IMvxCommandAsync CloseCommand =>
            mCloseCommand ?? (mCloseCommand = new MvxCommandAsync(async () => await NavigationService.Close(this).ConfigureAwait(true)));


        private IMvxCommand? mIncrementCommand;
        public IMvxCommand IncrementCommand =>
            mIncrementCommand ?? (mIncrementCommand = new MvxCommand(Increment));

        public DictionaryBindingViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }

        private void Increment()
        {
            if(Value < 3)
            {
                Value++;
            }
            else
            {
                Value = 0;
            }
        }
    }
}
