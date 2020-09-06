// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Mobile.Sample
{
    using System.Threading.Tasks;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using MvvmCross.Commands;
    using MvvmCross.Logging;
    using Nivaes.App.Cross.Sample;

    public class WindowChildParam
    {
        public int ParentNo { get; set; }
        public int ChildNo { get; set; }
    }

    public class WindowViewModel
        : MvxNavigationViewModel
    {
        private static int mCount;

        public string Title => $"No.{Count} Window View";

        private Modes mMode = Modes.Blue;
        public Modes Mode
        {
            get => mMode;
            set
            {
                if (value == mMode) return;
                mMode = value;
                RaisePropertyChanged(() => Mode);
            }
        }

        private bool mIsItem1 = true;
        public bool IsItem1
        {
            get => mIsItem1;
            set
            {
                if (value == mIsItem1) return;
                mIsItem1 = value;
                RaisePropertyChanged(() => IsItem1);
            }
        }

        private bool mIsItem2 = false;
        public bool IsItem2
        {
            get => mIsItem2;
            set
            {
                if (value == mIsItem2) return;
                mIsItem2 = value;
                RaisePropertyChanged(() => IsItem2);
            }
        }

        private bool mIsItem3 = false;
        public bool IsItem3
        {
            get => mIsItem3;
            set
            {
                if (value == mIsItem3) return;
                mIsItem3 = value;
                RaisePropertyChanged(() => IsItem3);
            }
        }

        private bool mIsItemSetting = true;
        public bool IsItemSetting
        {
            get => mIsItemSetting;
            set
            {
                if (value == mIsItemSetting) return;
                mIsItemSetting = value;
                RaisePropertyChanged(() => IsItemSetting);
            }
        }

        public int Count { get; set; }

        public WindowViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            mCount++;
            Count = mCount;

            ShowWindowChildCommand = new MvxAsyncCommand<int>(async no =>
            {
                await NavigationService.Navigate<WindowChildViewModel, WindowChildParam>(new WindowChildParam
                {
                    ParentNo = Count,
                    ChildNo = no
                }).ConfigureAwait(false);
            });

            CloseCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this));

            ToggleSettingCommand = new MvxAsyncCommand(async () => 
            {
                await Task.Run(() =>
                {
                    IsItemSetting = !IsItemSetting;
                });
            });
        }

        public IMvxAsyncCommand CloseCommand { get; private set; }
        public IMvxAsyncCommand<int> ShowWindowChildCommand { get; private set; }

        public IMvxAsyncCommand ToggleSettingCommand { get; private set; }    
    }
}
