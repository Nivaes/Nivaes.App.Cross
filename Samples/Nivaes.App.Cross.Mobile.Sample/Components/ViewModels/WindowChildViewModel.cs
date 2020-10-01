// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Mobile.Sample
{
    using System.Threading.Tasks;
    using MvvmCross.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class WindowChildViewModel
        : MvxNavigationViewModel<WindowChildParam>
    {
        private WindowChildParam? mParam;

        public WindowChildViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
        }

        public int ParentNo => mParam!.ParentNo;
        public string Text => $"I'm No.{mParam!.ChildNo}. My parent is No.{mParam.ParentNo}";

        public IMvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () => await NavigationService.Close(this).ConfigureAwait(true));

        public override ValueTask Prepare(WindowChildParam param)
        {
            mParam = param;
            return new ValueTask();
        }
    }
}
