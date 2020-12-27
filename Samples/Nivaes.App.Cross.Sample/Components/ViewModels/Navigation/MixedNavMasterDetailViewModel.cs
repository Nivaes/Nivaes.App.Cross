// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using System;
    using System.Collections.Generic;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross.Commands;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;

    public class MixedNavMasterDetailViewModel
        : MvxNavigationViewModel
    {
        private MenuItem? mMenuItem;
        private IMvxAsyncCommand<MenuItem>? mOnSelectedChangedCommand;

        public class MenuItem
        {
            public string? Title { get; set; }

            public string? Description { get; set; }
            public Type? ViewModelType { get; set; }
        }

        public MixedNavMasterDetailViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            Menu = new[] {
                new MenuItem { Title = "Root", Description = "The root page", ViewModelType = typeof(MixedNavMasterRootContentViewModel) },
                new MenuItem { Title = "Tabs", Description = "Tabbed detail page", ViewModelType = typeof(MixedNavTabsViewModel)},
                new MenuItem { Title = "Result", Description = "Open detail page with result", ViewModelType = typeof(MixedNavResultDetailViewModel)},
            };
        }

        public IEnumerable<MenuItem> Menu { get; set; }

        public MenuItem? SelectedMenu {
            get => mMenuItem;
            set {
                if (SetProperty(ref mMenuItem, value))
                    OnSelectedChangedCommand.Execute(value);
            }
        }

        private IMvxAsyncCommand<MenuItem> OnSelectedChangedCommand => mOnSelectedChangedCommand ??= new MvxAsyncCommand<MenuItem>(async (item) =>
                 {
                     if (item == null)
                         return false;

                     var vmType = item.ViewModelType;
                     return await NavigationService.Navigate(vmType).ConfigureAwait(false);
                 });
    }
}
