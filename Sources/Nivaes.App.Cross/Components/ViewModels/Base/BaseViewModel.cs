using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public interface IBaseViewModel
        : ICrossViewModel
    {
        string? Title { get; }

        ValidateController ValidateController { get; }

        IValidator? Validator { get; set; }
    }

    internal interface IInternalBaseViewModel
        : IBaseViewModel
    {
        void OnDataModeChanged(ExPropertyChangedEventArgs args);
    }

    public abstract class BaseViewModel
        : CrossNavigationViewModel, IBaseViewModel, IInternalBaseViewModel
    {
        protected BaseViewModel(CrossNavigationService navigationService, ILogger logger)
            : base(navigationService, logger)
        {
            ValidateController = new ValidateController(this);
        }

        public string? Title { get; protected set; }

        public ValidateController ValidateController { get; private set; }

        public IValidator? Validator { get; set; }

        public override void ViewAppeared()
        {
            base.ViewAppeared();

            ValidateController.Initialize();
        }

        void IInternalBaseViewModel.OnDataModeChanged(ExPropertyChangedEventArgs args)
        {
            OnDataModeChanged(args);
        }

        protected virtual void OnDataModeChanged(ExPropertyChangedEventArgs args)
        { }
    }

    public abstract class BaseViewModel<TParameter>
        : CrossNavigationViewModel<TParameter>, IBaseViewModel, IInternalBaseViewModel
    {
        public string? Title { get; protected set; }

        public ValidateController ValidateController { get; private set; }

        public IValidator? Validator { get; set; }

        protected BaseViewModel(CrossNavigationService navigationService, ILogger logger)
            : base(navigationService, logger)
        {
            ValidateController = new ValidateController(this);
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();

            ValidateController.Initialize();
        }

        void IInternalBaseViewModel.OnDataModeChanged(ExPropertyChangedEventArgs args)
        {
            OnDataModeChanged(args);
        }

        protected virtual void OnDataModeChanged(ExPropertyChangedEventArgs args)
        { }
    }

    public abstract class BaseViewModelResult<TResult>
        : CrossNavigationViewModelResult<TResult>, IBaseViewModel, IInternalBaseViewModel
    {
        public string? Title { get; protected set; }

        public ValidateController ValidateController { get; private set; }

        public IValidator? Validator { get; set; }

        protected BaseViewModelResult(CrossNavigationService navigationService, ILogger logger)
            : base(navigationService, logger)
        {
            ValidateController = new ValidateController(this);
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();

            ValidateController.Initialize();
        }

        void IInternalBaseViewModel.OnDataModeChanged(ExPropertyChangedEventArgs args)
        {
            OnDataModeChanged(args);
        }

        protected virtual void OnDataModeChanged(ExPropertyChangedEventArgs args)
        { }
    }

    public abstract class BaseViewModel<TParameter, TResult>
        : CrossNavigationViewModel<TParameter, TResult>, IBaseViewModel, IInternalBaseViewModel
    {
        public string? Title { get; protected set; }

        public ValidateController ValidateController { get; private set; }

        public IValidator? Validator { get; set; }

        protected BaseViewModel(CrossNavigationService navigationService, ILogger logger)
            : base(navigationService, logger)
        {
            ValidateController = new ValidateController(this);
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();

            ValidateController.Initialize();
        }

        void IInternalBaseViewModel.OnDataModeChanged(ExPropertyChangedEventArgs args)
        {
            OnDataModeChanged(args);
        }

        protected virtual void OnDataModeChanged(ExPropertyChangedEventArgs args)
        { }
    }
}
