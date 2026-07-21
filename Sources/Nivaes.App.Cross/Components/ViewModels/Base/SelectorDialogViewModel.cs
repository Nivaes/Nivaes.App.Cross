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
        protected SelectorDialogViewModel(ILogger logger)
          : base(logger)
        { }
    }

    public abstract class SelectorDialogViewModel<TParameter>
       : DialogViewModel<TParameter>, ISelectorDialogViewModel
            where TParameter : class
    {
        protected SelectorDialogViewModel(ILogger logger)
          : base(logger)
        { }
    }

    public abstract class SelectorDialogViewModelResult<TResult>
       : DialogViewModelResult<TResult>, ISelectorDialogViewModel
    {
        protected SelectorDialogViewModelResult(ILogger logger)
            : base(logger)
        { }
    }

    public abstract class SelectorDialogViewModel<TParameter, TResult>
        : DialogViewModel<TParameter, TResult>, ISelectorDialogViewModel
    {
        protected SelectorDialogViewModel(ILogger logger)
            : base(logger)
        { }
    }
}
