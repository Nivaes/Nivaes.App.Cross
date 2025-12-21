namespace Playground.Core.ViewModels
{
    using System.Collections;
    using Microsoft.Extensions.Logging;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class Page1ViewModel 
        : CrossNavigationViewModel
    {
        public MvxCommand<int> HeaderTappedCommand { get; }

        public Page1ViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            HeaderTappedCommand = new MvxCommand<int>(DoHeaderTappedCommand);

            var random = new Random();
            var sections = new List<SectionViewModel>();
            for (var section = 0; section < 5; section++)
            {
                var itemsCount = random.Next(0, 10);
                var items = new List<SectionItemViewModel>();
                for (var item = 0; item < itemsCount; item++)
                {
                    items.Add(new SectionItemViewModel
                    {
                        Title = $"Item {item}"
                    });
                }
                sections.Add(new SectionViewModel(items, section % 2 == 0)
                {
                    Title = $"Section {section}",
                    On = section % 2 != 0
                });
            }

            Sections = sections;
        }

        public List<SectionViewModel> Sections { get; }

        private void DoHeaderTappedCommand(int index)
        {
            System.Diagnostics.Debug.WriteLine($"Header {index} tapped");
        }

        public class SectionViewModel : CrossNotifyPropertyChanged, IEnumerable<SectionItemViewModel>
        {
            private List<SectionItemViewModel> _items;
            private string _title;
            private bool _on;

            public bool ShowsControl { get; }

            public string Title
            {
                get => _title;
                set => SetProperty(ref _title, value);
            }

            public bool On
            {
                get => _on;
                set => SetProperty(ref _on, value);
            }

            public SectionViewModel(IEnumerable<SectionItemViewModel> items, bool showsControl)
            {
                _items = new List<SectionItemViewModel>(items);
                ShowsControl = showsControl;
            }

            public IEnumerator<SectionItemViewModel> GetEnumerator()
            {
                return _items.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class SectionItemViewModel : CrossNotifyPropertyChanged
        {
            private string _title;

            public string Title
            {
                get => _title;
                set => SetProperty(ref _title, value);
            }
        }
    }
}
