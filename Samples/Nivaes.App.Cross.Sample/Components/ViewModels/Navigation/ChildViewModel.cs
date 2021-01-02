// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Commands;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;
    using Nivaes.App.Cross.ViewModels;

    public class ChildViewModel
        : MvxNavigationViewModel<SampleModel, SampleModel>
    {
        public string BrokenTextValue { get => mBrokenTextValue; set => SetProperty(ref mBrokenTextValue, value); }
        public string AnotherBrokenTextValue { get => mAnotherBrokenTextValue; set => SetProperty(ref mAnotherBrokenTextValue, value); }

        private SampleModel? mParameter;
        private string mBrokenTextValue = string.Empty;
        private string mAnotherBrokenTextValue = string.Empty;

        public ChildViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            CloseCommand = new MvxCommandAsync(async () => await NavigationService.Close(this, new SampleModel
            {
                Message = "This returned correctly",
                Value = 5.67m
            }).ConfigureAwait(true));

            ShowSecondChildCommand = new MvxCommandAsync(async () => await NavigationService.Navigate<SecondChildViewModel>().ConfigureAwait(true));

            ShowRootCommand = new MvxCommandAsync(async () => await NavigationService.Navigate<RootViewModel>().ConfigureAwait(true));

            PropertyChanged += ChildViewModelPropertyChanged;
        }

        private void ChildViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Demonstrates that exceptions can be raised on property changed but are swallowed by default to 
            // protect the app from crashing
            if (e.PropertyName == nameof(BrokenTextValue))
                throw new System.NotImplementedException();
        }

        public override ValueTask Prepare()
        {
            return base.Prepare();
        }

        public override ValueTask Prepare(SampleModel parameter)
        {
            mParameter = parameter;
            return new ValueTask();
        }

        protected override ValueTask SaveStateToBundle(IMvxBundle? bundle)
        {
            return base.SaveStateToBundle(bundle);
        }

        protected override ValueTask ReloadFromBundle(IMvxBundle? state)
        {
            return base.ReloadFromBundle(state);
        }

        public override async ValueTask Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            await Task.Delay(8500).ConfigureAwait(false);
        }

        public override ValueTask Start()
        {
            return base.Start();
        }

        public IMvxCommandAsync CloseCommand { get; private set; }

        public IMvxCommandAsync ShowSecondChildCommand { get; private set; }

        public IMvxCommandAsync ShowRootCommand { get; private set; }

        public override async ValueTask ViewAppeared()
        {
            await base.ViewAppeared().ConfigureAwait(true);

            await Task.Delay(1000).ConfigureAwait(true);
            BrokenTextValue = "This will throw exception in UI layer";
            AnotherBrokenTextValue = "This will throw exception in page";
        }
    }
}
