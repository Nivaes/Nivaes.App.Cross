using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public interface ISelectorDialogViewModel
        : IDialogViewModel
    {
    }

    public abstract class SelectorDialogViewModel
        : DialogViewModel, ISelectorDialogViewModel
    {
        protected SelectorDialogViewModel(ICrossNavigationService navigationService, ILogger logger)
          : base(navigationService, logger)
        { }
    }

    public abstract class SelectorDialogViewModel<TParameter>
       : DialogViewModel<TParameter>, ISelectorDialogViewModel
            where TParameter : class
    {
        protected SelectorDialogViewModel(ICrossNavigationService navigationService, ILogger logger)
          : base(navigationService, logger)
        { }
    }

    public abstract class SelectorDialogViewModelResult<TResult>
       : DialogViewModelResult<TResult>, ISelectorDialogViewModel
    {
        protected SelectorDialogViewModelResult(ICrossNavigationService navigationService, ILogger logger)
            : base(navigationService, logger)
        { }
    }

    public abstract class SelectorDialogViewModel<TParameter, TResult>
        : DialogViewModel<TParameter, TResult>, ISelectorDialogViewModel
    {
        protected SelectorDialogViewModel(ICrossNavigationService navigationService, ILogger logger)
            : base(navigationService, logger)
        { }
    }
}
