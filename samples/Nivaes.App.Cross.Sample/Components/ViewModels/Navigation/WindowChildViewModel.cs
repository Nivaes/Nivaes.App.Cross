using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class WindowChildViewModel
    : MvxNavigationViewModel<WindowChildParam>
{
    private WindowChildParam? _param;

    public WindowChildViewModel(ILogger<WindowChildViewModel> logger, ICrossNavigationService navigationService)
        : base(logger, navigationService)
    {
    }

    public int? ParentNo => _param?.ParentNo;
    public string Text => $"I'm No.{_param?.ChildNo}. My parent is No.{_param.ParentNo}";

    public ICrossAsyncCommand CloseCommand => new CrossAsyncCommand(async () => await NavigationService.Close(this));

    public override void Prepare(WindowChildParam param) => _param = param;
}
