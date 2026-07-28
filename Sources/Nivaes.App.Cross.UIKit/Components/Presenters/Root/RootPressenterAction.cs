using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public sealed class RootPressenterAction
            : PressenterAction<RootPresentationAttribute>
    {
        #region Constructor
        public RootPressenterAction(
                IPressenterActionContext context,
                IMvxIosViewCreator viewCreator,
                ILogger<RootPressenterAction> logger)
            : base(context, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(IViewModelRequest request, RootPresentationAttribute attribute)
        {
            var viewController = (UIViewController?)ViewCreator.CreateView(request);
            if (viewController == null)
            {
                Logger.LogWarning("Got null ViewController for request {Request}", request);

                return ValueTask.FromResult(false);
            }
            return ShowRootViewController(viewController, attribute, request);
        }

        protected override ValueTask<bool> CloseAction(IViewModelRequest request, RootPresentationAttribute attribute)
        {
            Logger.LogWarning($"Ignored attempt to close the window root (ViewModel type: {request.ViewModelType.FullName})");

            return ValueTask.FromResult(false);
        }
    }
}
