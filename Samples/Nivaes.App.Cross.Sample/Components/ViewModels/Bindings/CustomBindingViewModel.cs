// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using System;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;

    public class CustomBindingViewModel
        : MvxNavigationViewModel
    {
        private IMvxAsyncCommand? mCloseCommand;

        private int _counter = 2;

        private DateTime _date = DateTime.Now;

        private string _hello = "Hello MvvmCross";

        public CustomBindingViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }

        public string Hello
        {
            get => _hello;
            set => SetProperty(ref _hello, value);
        }

        public IMvxAsyncCommand CloseCommand => mCloseCommand ??
                                                (mCloseCommand = new MvxAsyncCommand(async () =>
                                                    await NavigationService.Close(this).ConfigureAwait(false)));

        public int Counter
        {
            get => _counter;
            set => SetProperty(ref _counter, value);
        }

        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }
    }
}
