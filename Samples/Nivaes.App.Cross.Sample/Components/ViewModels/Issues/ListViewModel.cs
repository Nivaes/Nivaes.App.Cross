// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using System.Threading.Tasks;
    using Nivaes.App.Cross.ViewModels;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Commands;

    public class ListViewModel
        : MvxViewModel
    {
        public MvxObservableCollection<TestItem> TestItems { get; } = new MvxObservableCollection<TestItem>();
        public IMvxCommandAsync<TestItem> ItemClickedCommand => new MvxCommandAsync<TestItem>(ItemClicked);

        public ListViewModel()
        {
            TestItems.Add(new TestItem()
            {
                Title = "Item1"
            });

            TestItems.Add(new TestItem()
            {
                Title = "Item2"
            });
        }

        private ValueTask<bool> ItemClicked(TestItem item)
        {
            return new ValueTask<bool>(true);
        }
    }

    public class TestItem
    {
        public string Title { get; set; } = string.Empty;
    }
}
