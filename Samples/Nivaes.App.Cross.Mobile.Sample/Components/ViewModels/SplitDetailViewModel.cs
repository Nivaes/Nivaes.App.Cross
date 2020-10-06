// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Mobile.Sample
{
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class SplitDetailViewModel
        : MvxNavigationViewModel
    {
        public SplitDetailViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            ShowChildCommand = new MvxAsyncNavigationCommand<SplitDetailNavViewModel>();
            ShowTabsCommand = new MvxAsyncNavigationCommand<TabsRootBViewModel>();
            ShowTabbedChildCommand = new MvxAsyncNavigationCommand<TabsRootViewModel>();
        }

        public IMvxAsyncCommand ShowChildCommand { get; }
        public IMvxAsyncCommand ShowTabsCommand { get; }
        public IMvxAsyncCommand ShowTabbedChildCommand { get; }

        public string ContentText => "Text for the Content Area";

        public override ValueTask ViewAppeared()
        {
            return base.ViewAppeared();
        }
    }
}
