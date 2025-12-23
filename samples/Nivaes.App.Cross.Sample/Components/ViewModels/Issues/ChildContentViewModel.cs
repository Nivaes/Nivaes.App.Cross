namespace Playground.Core.ViewModels
{
    using System.Threading.Tasks;
    using Nivaes.App.Cross;

    public class ChildContentViewModel
        : CrossViewModel
    {
        public ChildContentViewModel()
        {
        }

        private string _test;

        public string Test
        {
            get { return _test; }
            set { SetProperty(ref _test, value); }
        }

        public override async Task Initialize()
        {
            //Test = "Bound Text";
            await Task.Yield();
        }
    }
}
