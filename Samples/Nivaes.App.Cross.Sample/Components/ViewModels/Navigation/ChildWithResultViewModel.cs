using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public sealed class ChildWithResultViewModel(
        ILogger<ChildWithResultViewModel> logger,
        CrossNavigationService navigationService,
        ICrossResultViewModelManager resultViewModelManager)
    : CrossNavigationResultSettingViewModel<SampleModel, SampleModel>(
        logger,
        navigationService,
        resultViewModelManager)
{
    private SampleModel _model = null!;

    public CrossAsyncCommand CloseCommand => new(DoClose);

    public string Message
    {
        get => _model.Message;
        set
        {
            _model = _model with { Message = value };
            RaisePropertyChanged();
        }
    }

    public decimal Value
    {
        get => _model.Value;
        set
        {
            _model = _model with { Value = value };
            RaisePropertyChanged();
        }
    }

    private Task DoClose()
    {
        return NavigationService.CloseSettingResult(this, _model);
    }

    public override void Prepare(SampleModel parameter)
    {
        _model = parameter;
    }
}