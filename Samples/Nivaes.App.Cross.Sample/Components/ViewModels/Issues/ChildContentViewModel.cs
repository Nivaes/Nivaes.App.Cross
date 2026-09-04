using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample
{
    public class ChildContentViewModel
        : CrossViewModel
    {
        public ChildContentViewModel(ILogger<ChildContentViewModel> logger)
            : base(logger)
        {
        }

        private string? _test;

        public string? Test
        {
            get { return _test; }
            set { SetProperty(ref _test, value); }
        }

        public override async ValueTask Initialize()
        {
            //Test = "Bound Text";
            await Task.Yield();
        }
    }
}
