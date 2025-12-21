namespace Playground.Core.ViewModels
{
    using System.Threading.Tasks;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class ListViewModel : CrossViewModel
    {
        public CrossObservableCollection<TestItem> TestItems { get; } = new CrossObservableCollection<TestItem>();
        public ICrossAsyncCommand<TestItem> ItemClickedCommand => new MvxAsyncCommand<TestItem>(ItemClicked);

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

        private async Task ItemClicked(TestItem arg)
        {
            var item = arg;
        }
    }

    public class TestItem
    {
        public string Title { get; set; }
    }
}
