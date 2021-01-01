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

    public class WindowChildViewModel
        : MvxNavigationViewModel<WindowChildParam>
    {
        private WindowChildParam? mParam;

        public WindowChildViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }

        public int ParentNo => mParam!.ParentNo;
        public string Text => $"I'm No.{mParam!.ChildNo}. My parent is No.{mParam.ParentNo}";

        public IMvxCommandAsync CloseCommand => new MvxCommandAsync(async () => await NavigationService.Close(this).ConfigureAwait(true));

        public override ValueTask Prepare(WindowChildParam param)
        {
            mParam = param;
            return new ValueTask();
        }
    }
}
