using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.AppKitLib
{
    public sealed class MvxMacViewDispatcher
        : MvxMacUIThreadDispatcher
        , ICrossViewDispatcher
    {
        private readonly IMacViewPresenterManager _presenter;

        public MvxMacViewDispatcher(IMacViewPresenterManager presenter, ILogger<MvxMacViewDispatcher> logger)
            : base(logger)
        {
            _presenter = presenter;
        }

        public async Task<bool> ShowViewModel(ViewModelRequest request)
        {
            Func<Task> action = () =>
            {
                Logger.LogTrace($"Navigate requested{request.ViewModelType!.FullName}");
                return _presenter.Show(request).AsTask();
            };
            await ExecuteOnMainThreadAsync(action);
            return true;
        }

        public async Task<bool> ChangePresentation(CrossPresentationHint hint)
        {
            Func<Task> action = () =>
            {
                Logger.LogTrace($"Change presentation requested");
                return _presenter.ChangePresentation(hint).AsTask();
            };
            await ExecuteOnMainThreadAsync(action);
            return true;
        }


        //public ValueTask<bool> ShowViewModel(CrossViewModelRequest request)
        //{
        //    return ExecuteOnMainThreadAsync(() =>
        //    {
        //        Logger.LogTrace($"Navigate requested{request.ViewModelType!.FullName}");
        //        return _presenter.Show(request);
        //    });
        //}

        //public ValueTask<bool> ChangePresentation(CrossPresentationHint hint)
        //{
        //    return ExecuteOnMainThreadAsync(() =>
        //    {
        //        Logger.LogTrace("Change presentation requested");
        //        return _presenter.ChangePresentation(hint);
        //    });
        //}


    }
}
