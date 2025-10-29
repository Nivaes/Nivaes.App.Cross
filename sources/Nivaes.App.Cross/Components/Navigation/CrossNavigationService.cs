namespace Nivaes.App.Cross
{
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    public sealed class CrossNavigationService : 
        ICrossNavigationService
    {
        private ConditionalWeakTable<ICrossViewModel, TaskCompletionSource<object>> _tcsResults = new ConditionalWeakTable<ICrossViewModel, TaskCompletionSource<object>>();

        public readonly ICrossViewDispatcher mViewDispatcher;

        public event BeforeNavigateEventHandler? BeforeNavigate;

        public event AfterNavigateEventHandler? AfterNavigate;

        public event BeforeCloseEventHandler? BeforeClose;

        public event AfterCloseEventHandler? AfterClose;

        public event BeforeChangePresentationEventHandler? BeforeChangePresentation;

        public event AfterChangePresentationEventHandler? AfterChangePresentation;


        public CrossNavigationService(ICrossViewDispatcher viewDispatcher)
        {
            mViewDispatcher = viewDispatcher;
        }

        public async Task<bool> Navigate<TViewModel>(ICrossBundle? presentationBundle = null, CancellationToken cancellationToken = default)
            where TViewModel : ICrossViewModel
        {
            var viewModel = CrossViewModelLoader.LoadViewModel<TViewModel>();
            var request = new CrossViewModelRequest<TViewModel>(viewModel);

            var hasNavigated = false;

            var args = new CrossNavigateEventArgs(viewModel, CrossNavigationMode.Show, cancellationToken);
            OnBeforeNavigate(this, args);

            if (args.Cancel)
                return false;

            hasNavigated = await mViewDispatcher.ShowViewModel(request).ConfigureAwait(false);
            if (!hasNavigated)
                return false;

            if (viewModel.InitializeTask?.Task != null)
                await viewModel.InitializeTask.Task.ConfigureAwait(false);

            OnAfterNavigate(this, args);
            return true;
        }

        public Task<bool> Navigate<TViewModel, TParameter>(TParameter parameter, 
                ICrossBundle? presentationBundle = null, ICrossNavigateEventArgs? args = null, 
                CancellationToken cancellationToken = default)
            where TViewModel : ICrossViewModel<TParameter, bool>
        {
            var aa = Navigate<TViewModel, TParameter, bool>(parameter, presentationBundle, args, cancellationToken);

            return aa;
        }

        public async Task<TResult?> Navigate<TViewModel, TResult>(ICrossBundle? presentationBundle = null,
            CancellationToken cancellationToken = default)
           where TViewModel : ICrossViewModelResult<TResult>
        {
            var hasNavigated = false;
            var tcs = new TaskCompletionSource<object>();

            var viewModel = CrossViewModelLoader.LoadViewModel<TViewModel>();
            var request = new CrossViewModelRequest<TViewModel>(viewModel);

            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(async () =>
                {
                    if (hasNavigated && !tcs.Task.IsCompleted)
                        await Close(viewModel, default(TResult));
                });
            }

            var args = new CrossNavigateEventArgs(viewModel, CrossNavigationMode.Show, cancellationToken);
            OnBeforeNavigate(this, args);

            viewModel.CloseCompletionSource = tcs;
            _tcsResults.Add(viewModel, tcs);

            if (cancellationToken.IsCancellationRequested)
                return default(TResult);

            hasNavigated = await mViewDispatcher.ShowViewModel(request);
            if (!hasNavigated)
                return default(TResult);

            if (viewModel.InitializeTask?.Task != null)
                await viewModel.InitializeTask.Task.ConfigureAwait(false);

            OnAfterNavigate(this, args);

            try
            {
                return (TResult)await tcs.Task;
            }
            catch (Exception)
            {
                return default(TResult);
            }
        }

        public async Task<TResult?> Navigate<TViewModel, TParameter, TResult>(TParameter parameter, 
            ICrossBundle? presentationBundle = null, ICrossNavigateEventArgs? args = null,
            CancellationToken cancellationToken = default)
            where TViewModel : ICrossViewModel<TParameter, TResult>
        {
            var hasNavigated = false;

            var viewModel = CrossViewModelLoader.LoadViewModel<TViewModel>();
            var request = new CrossViewModelRequest<TViewModel>(viewModel);

            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(async () =>
                {
                    if (hasNavigated)
                        await Close(viewModel, default(TResult));
                });
            }

            if (args == null)
            {
                args = new CrossNavigateEventArgs(viewModel, CrossNavigationMode.Show, cancellationToken);
            }
            OnBeforeNavigate(this, args);

            var tcs = new TaskCompletionSource<object>();
            viewModel.CloseCompletionSource = tcs;
            _tcsResults.Add(viewModel, tcs);

            if (cancellationToken.IsCancellationRequested)
                return default(TResult);

            hasNavigated = await mViewDispatcher.ShowViewModel(request);

            if (viewModel.InitializeTask?.Task != null)
                await viewModel.InitializeTask.Task.ConfigureAwait(false);

            OnAfterNavigate(this, args);

            try
            {
                return (TResult)await tcs.Task;
            }
            catch (Exception)
            {
                return default(TResult);
            }
        }     

        public async Task<bool> ChangePresentation(CrossPresentationHint hint, CancellationToken cancellationToken = default)
        {
            var args = new CrossChangePresentationEventArgs(hint, cancellationToken);
            OnBeforeChangePresentation(this, args);

            if (args.Cancel)
                return false;

            var result = await mViewDispatcher.ChangePresentation(hint);

            args.Result = result;
            OnAfterChangePresentation(this, args);

            return result;
        }

        public async Task<bool> Close(ICrossViewModel viewModel, CancellationToken? cancellationToken = default)
        {
            var args = new CrossNavigateEventArgs(viewModel, CrossNavigationMode.Close, cancellationToken);
            OnBeforeClose(this, args);

            if (args.Cancel)
                return false;

            var close = await mViewDispatcher.ChangePresentation(new CrossClosePresentationHint(viewModel));
            OnAfterClose(this, args);

            return close;
        }

        public async Task<bool> Close<TResult>(ICrossViewModelResult<TResult> viewModel, TResult result, CancellationToken? cancellationToken = default)
        {
            _tcsResults.TryGetValue(viewModel, out var _tcs);

            //Disable cancelation of the Task when closing ViewModel through the service
            viewModel.CloseCompletionSource = null;

            try
            {
                var closeResult = await Close(viewModel, cancellationToken);
                if (closeResult)
                {
                    _tcs?.TrySetResult(result!);
                    _tcsResults.Remove(viewModel);
                }
                else
                    viewModel.CloseCompletionSource = _tcs;
                return closeResult;
            }
            catch (Exception ex)
            {
                _tcs?.TrySetException(ex);
                return false;
            }
        }

        private void OnBeforeNavigate(object sender, ICrossNavigateEventArgs e)
        {
            BeforeNavigate?.Invoke(sender, e);
        }

        private void OnAfterNavigate(object sender, ICrossNavigateEventArgs e)
        {
            AfterNavigate?.Invoke(sender, e);
        }

        private void OnBeforeClose(object sender, ICrossNavigateEventArgs e)
        {
            BeforeClose?.Invoke(sender, e);
        }

        private void OnAfterClose(object sender, ICrossNavigateEventArgs e)
        {
            AfterClose?.Invoke(sender, e);
        }

        private void OnBeforeChangePresentation(object sender, CrossChangePresentationEventArgs e)
        {
            BeforeChangePresentation?.Invoke(sender, e);
        }

        private void OnAfterChangePresentation(object sender, CrossChangePresentationEventArgs e)
        {
            AfterChangePresentation?.Invoke(sender, e);
        }
    }
}
