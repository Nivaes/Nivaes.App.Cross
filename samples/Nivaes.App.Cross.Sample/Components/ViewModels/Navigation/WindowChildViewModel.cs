namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class WindowChildViewModel : MvxNavigationViewModel<WindowChildParam>
    {
        private WindowChildParam _param;

        public WindowChildViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }

        public int ParentNo => _param.ParentNo;
        public string Text => $"I'm No.{_param.ChildNo}. My parent is No.{_param.ParentNo}";

        public ICrossAsyncCommand CloseCommand => new CrossAsyncCommand(async () => await NavigationService.Close(this));

        public override void Prepare(WindowChildParam param) => _param = param;
    }
}
