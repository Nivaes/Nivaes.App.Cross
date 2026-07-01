using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class WindowChildParam
{
    public int ParentNo { get; set; }
    public int ChildNo { get; set; }
}

public class WindowViewModel
    : CrossNavigationViewModel
{
    private static int _count;

    public string Title => $"No.{Count} Window View";

    private Modes _mode = Modes.Blue;
    public Modes Mode
    {
        get { return _mode; }
        set
        {
            if (value == _mode) return;
            _mode = value;
            RaisePropertyChanged(() => Mode);
        }
    }

    private bool _isItem1 = true;
    public bool IsItem1
    {
        get { return _isItem1; }
        set
        {
            if (value == _isItem1) return;
            _isItem1 = value;
            RaisePropertyChanged(() => IsItem1);
        }
    }

    private bool _isItem2 = false;
    public bool IsItem2
    {
        get { return _isItem2; }
        set
        {
            if (value == _isItem2) return;
            _isItem2 = value;
            RaisePropertyChanged(() => IsItem2);
        }
    }

    private bool _isItem3 = false;
    public bool IsItem3
    {
        get { return _isItem3; }
        set
        {
            if (value == _isItem3) return;
            _isItem3 = value;
            RaisePropertyChanged(() => IsItem3);
        }
    }

    private bool _isItemSetting = true;
    public bool IsItemSetting
    {
        get { return _isItemSetting; }
        set
        {
            if (value == _isItemSetting) return;
            _isItemSetting = value;
            RaisePropertyChanged(() => IsItemSetting);
        }
    }

    public int Count { get; set; }

    public WindowViewModel(ILogger<WindowViewModel> logger, ICrossNavigationService navigationService)
        : base(navigationService, logger)
    {
        _count++;
        Count = _count;

        ShowWindowChildCommand = new MvxAsyncCommand<int>(async no =>
        {
            await NavigationService.Navigate<WindowChildViewModel, WindowChildParam>(new WindowChildParam
            {
                ParentNo = Count,
                ChildNo = no
            });
        });

        CloseCommand = new CrossAsyncCommand(() => NavigationService.Close(this));

        ToggleSettingCommand = new CrossAsyncCommand(async () =>
        {
            await Task.Run(() =>
            {
                IsItemSetting = !IsItemSetting;
            });
        });
    }

    public ICrossAsyncCommand CloseCommand { get; }
    public ICrossAsyncCommand<int> ShowWindowChildCommand { get; }
    public ICrossAsyncCommand ToggleSettingCommand { get; }
}
