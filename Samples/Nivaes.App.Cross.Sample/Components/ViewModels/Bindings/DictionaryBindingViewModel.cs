// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using Nivaes.App.Cross.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class DictionaryBindingViewModel
        : MvxNavigationViewModel
    {
        private int mValue;
        public int Value
        {
            get => mValue;
            set => SetProperty(ref mValue, value);
        }

        private IMvxAsyncCommand? mCloseCommand;
        public IMvxAsyncCommand CloseCommand =>
            mCloseCommand ?? (mCloseCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this).ConfigureAwait(true)));


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
