using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample
{
    public class ListViewModel : CrossViewModel
    {
        public CrossObservableCollection<TestItem> TestItems { get; } = new CrossObservableCollection<TestItem>();
        public ICrossAsyncCommand<TestItem> ItemClickedCommand => new CrossAsyncCommand<TestItem>(ItemClicked);

        public ListViewModel(ILogger<ListViewModel> logger)
            : base(logger)
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

        private async Task ItemClicked(TestItem? arg)
        {
            var item = arg;
        }
    }

    public class TestItem
    {
        public string? Title { get; set; }
    }
}
