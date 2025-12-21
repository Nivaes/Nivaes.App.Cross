namespace Playground.Core.ViewModels.Navigation
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.ViewModels;
    using MvvmCross.ViewModels.Result;
    using Nivaes.App.Cross;
    using Playground.Core.Models;

    public sealed class ChildWithResultViewModel(
            ILoggerFactory logFactory,
            ICrossNavigationService navigationService,
            IMvxResultViewModelManager resultViewModelManager)
        : MvxNavigationResultSettingViewModel<SampleModel, SampleModel>(
            logFactory,
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
}